using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementService.CommonLib.Exceptions;
using TaskManagementService.CommonLib.Filters;

namespace TaskManagementService.CommonLib.Filters;

public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly IDictionary<Type, Action<ExceptionContext>> exceptionHandlers;
    private readonly ILogger<ApiExceptionFilterAttribute>? logger;

    public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
    {
        this.logger = logger;

        exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(TimeoutException), ctx => Write(ctx, StatusCodes.Status408RequestTimeout, "Время ожидания истекло", detail: "Операция не была завершена в отведенное время") },
                { typeof(FluentValidation.ValidationException), HandleFluentValidationException },
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), ctx => Write(ctx, StatusCodes.Status404NotFound, "Запрашиваемый ресурс не найден", type: "https://tools.ietf.org/html/rfc7231#section-6.5.4", detail: ctx.Exception.Message) },
                { typeof(UnauthorizedAccessException), ctx => Write(ctx, StatusCodes.Status401Unauthorized, "Неавторизованный доступ", type: "https://tools.ietf.org/html/rfc7235#section-3.1", detail: "Требуется авторизация для доступа к этому ресурсу") },
                { typeof(ForbiddenAccessException), ctx => Write(ctx, StatusCodes.Status403Forbidden, "Доступ запрещён", type: "https://tools.ietf.org/html/rfc7231#section-6.5.3", detail: "Недостаточно прав для выполнения операции") },
                { typeof(BadRequestException), ctx => Write(ctx, StatusCodes.Status400BadRequest, "Некорректный запрос", detail: ctx.Exception.Message) },
                { typeof(InternalServerErrorException), ctx => Write(ctx, StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера", type: "https://tools.ietf.org/html/rfc7231#section-6.6.1", detail: GetSafeErrorMessage(ctx.Exception)) },
                { typeof(AlreadyExistsException), ctx => Write(ctx, StatusCodes.Status409Conflict, "Конфликт", type: "https://tools.ietf.org/html/rfc7231#section-6.5.8", detail: ctx.Exception.Message) },
                { typeof(UnprocessableEntityException), ctx => Write(ctx, StatusCodes.Status422UnprocessableEntity, "Неподдерживаемый контент", detail: ctx.Exception.Message) },
                { typeof(IdentityException), ctx => Write(ctx, StatusCodes.Status401Unauthorized, "Неавторизованный доступ", detail: "Требуется авторизация для доступа к этому ресурсу") },
                { typeof(ExternalApiException), HandleExternalApiException },
                { typeof(TaskCanceledException), HandleTaskCanceledException },
                { typeof(OperationCanceledException), ctx => Write(ctx, StatusCodes.Status499ClientClosedRequest, "Операция отменена", detail: "Выполнение операции было прервано") },
                { typeof(ArgumentException), ctx => Write(ctx, StatusCodes.Status400BadRequest, "Некорректный аргумент", detail: ctx.Exception.Message) },
                { typeof(ArgumentNullException), ctx => Write(ctx, StatusCodes.Status400BadRequest, "Отсутствует обязательный параметр", detail: "Один из обязательных параметров не указан") },
                { typeof(InvalidOperationException), ctx => Write(ctx, StatusCodes.Status422UnprocessableEntity, "Недопустимая операция", detail: ctx.Exception.Message) },
                { typeof(NotSupportedException), ctx => Write(ctx, StatusCodes.Status501NotImplemented, "Операция не поддерживается", type: "https://tools.ietf.org/html/rfc7231#section-6.6.2", detail: "Запрашиваемая операция не реализована") },
                { typeof(DbUpdateException), ctx => Write(ctx, StatusCodes.Status500InternalServerError, "Ошибка базы данных", detail: "Произошла ошибка при обновлении данных") },
                { typeof(DbUpdateConcurrencyException), ctx => Write(ctx, StatusCodes.Status409Conflict, "Конфликт параллельного доступа", detail: "Данные были изменены другим пользователем. Обновите страницу и повторите операцию") },
                { typeof(TooManyRequestsException), HandleTooManyRequestsException },
                { typeof(ConcurrencyException), ctx => Write(ctx, StatusCodes.Status409Conflict, "Конфликт параллельного доступа", detail: ctx.Exception.Message) },
                { typeof(ServiceUnavailableException), HandleServiceUnavailableException },
                { typeof(BusinessRuleViolationException), HandleBusinessRuleViolationException }
            };
    }

    public override void OnException(ExceptionContext context)
    {
        LogException(context);
        HandleException(context);
        base.OnException(context);
    }

    private void LogException(ExceptionContext context)
    {
        var exception = context.Exception;
        var httpContext = context.HttpContext;
        var logLevel = GetLogLevel(exception);

        if (logger?.IsEnabled(logLevel) == true)
        {
            logger.Log(logLevel, exception, "API Exception: {Type} {Message} Path:{Path} Trace:{TraceId}",
                exception.GetType().Name, exception.Message, httpContext.Request.Path, httpContext.TraceIdentifier);
        }
    }

    private static LogLevel GetLogLevel(Exception exception) => exception switch
    {
        NotFoundException or BadRequestException or ValidationException or FluentValidation.ValidationException or ArgumentException or ArgumentNullException => LogLevel.Warning,
        UnauthorizedAccessException or ForbiddenAccessException or IdentityException => LogLevel.Warning,
        InternalServerErrorException or ExternalApiException or DbUpdateException or DbUpdateConcurrencyException => LogLevel.Error,
        TaskCanceledException or OperationCanceledException => LogLevel.Information,
        _ => LogLevel.Error
    };

    private void HandleException(ExceptionContext context)
    {
        var type = context.Exception.GetType();
        if (exceptionHandlers.TryGetValue(type, out var handler))
        {
            handler(context);
            return;
        }

        foreach (var kv in exceptionHandlers)
        {
            if (kv.Key.IsAssignableFrom(type))
            {
                kv.Value(context);
                return;
            }
        }

        if (!context.ModelState.IsValid)
        {
            HandleInvalidModelStateException(context);
            return;
        }

        Write(context, StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера", detail: "Произошла непредвиденная ошибка");
    }

    private static void Write(ExceptionContext ctx,
                              int status,
                              string title,
                              string? type = null,
                              string? detail = null,
                              IDictionary<string, object?>? extensions = null)
    {
        var pd = ApiProblemDetailsFactory.Create(ctx.HttpContext, status, title, type, detail, ctx.Exception, extensions);
        ctx.Result = new ObjectResult(pd) { StatusCode = status };
        ctx.ExceptionHandled = true;
    }

    private void HandleValidationException(ExceptionContext context)
        => Write(context, StatusCodes.Status400BadRequest, "Ошибка валидации", type: "https://tools.ietf.org/html/rfc7231#section-6.5.1");

    private void HandleFluentValidationException(ExceptionContext context)
        => Write(context, StatusCodes.Status400BadRequest, "Ошибка валидации", type: "https://tools.ietf.org/html/rfc7231#section-6.5.1");

    private void HandleInvalidModelStateException(ExceptionContext context)
    {
        var details = new ValidationProblemDetails(context.ModelState)
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Title = "Ошибка валидации модели",
            Instance = context.HttpContext.Request.Path
        };
        context.Result = new BadRequestObjectResult(details);
        context.ExceptionHandled = true;
    }

    private void HandleExternalApiException(ExceptionContext context)
    {
        var apiException = (ExternalApiException)context.Exception;
        var extensions = new Dictionary<string, object?>
            {
                { "externalStatus", apiException.StatusCode },
                { "externalServiceName", apiException.ServiceName },
                { "externalDetail", apiException.Message },
                { "timestamp", DateTimeOffset.UtcNow }
            };
        Write(context, StatusCodes.Status502BadGateway, "Ошибка внешнего сервиса", type: "https://tools.ietf.org/html/rfc7231#section-6.6.3", detail: "Временная недоступность внешнего сервиса", extensions: extensions);
    }

    private void HandleTaskCanceledException(ExceptionContext context)
    {
        var canceledException = (TaskCanceledException)context.Exception;
        var isTimeout = canceledException.InnerException is TimeoutException;
        Write(context,
            isTimeout ? StatusCodes.Status408RequestTimeout : StatusCodes.Status499ClientClosedRequest,
            isTimeout ? "Время ожидания истекло" : "Запрос отменен",
            detail: "Операция была отменена");
    }

    private void HandleTooManyRequestsException(ExceptionContext context)
    {
        var ex = (TooManyRequestsException)context.Exception;
        var retry = ex.RetryAfter.HasValue ? (int?)ex.RetryAfter.Value.TotalSeconds : null;
        if (retry.HasValue)
            context.HttpContext.Response.Headers.RetryAfter = retry.Value.ToString();
        Write(context, StatusCodes.Status429TooManyRequests, "Слишком много запросов", type: "https://tools.ietf.org/html/rfc6585#section-4", detail: ex.Message);
    }

    private void HandleServiceUnavailableException(ExceptionContext context)
    {
        var ex = (ServiceUnavailableException)context.Exception;
        if (ex.RetryAfter.HasValue)
            context.HttpContext.Response.Headers.RetryAfter = ((int)ex.RetryAfter.Value.TotalSeconds).ToString();
        Write(context, StatusCodes.Status503ServiceUnavailable, "Сервис недоступен", type: "https://tools.ietf.org/html/rfc7231#section-6.6.4", detail: ex.Message);
    }

    private void HandleBusinessRuleViolationException(ExceptionContext context)
    {
        var ex = (BusinessRuleViolationException)context.Exception;
        var extensions = new Dictionary<string, object?>();
        if (!string.IsNullOrEmpty(ex.RuleCode)) extensions.Add("ruleCode", ex.RuleCode);
        Write(context, StatusCodes.Status422UnprocessableEntity, "Нарушение бизнес-правила", detail: ex.Message, extensions: extensions);
    }

    private static string GetSafeErrorMessage(Exception? exception) => exception?.Message ?? "Произошла внутренняя ошибка сервера";
}
