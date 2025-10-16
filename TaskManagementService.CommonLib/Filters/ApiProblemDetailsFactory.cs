using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TaskManagementService.CommonLib.Filters;

/// <summary>
/// Central helper for unified ProblemDetails / ValidationProblemDetails creation.
/// Encapsulates common fields population and extension merging.
/// </summary>
public static class ApiProblemDetailsFactory
{
    public static ProblemDetails Create(HttpContext httpContext,
        int status,
        string title,
        string? type = null,
        string? detail = null,
        Exception? exception = null,
        IDictionary<string, object?>? extensions = null)
    {
        ProblemDetails details;

        if (status == StatusCodes.Status400BadRequest)
        {
            if (exception is FluentValidation.ValidationException fluentEx)
            {
                details = new ValidationProblemDetails(fluentEx.Errors
                    .GroupBy(e => string.IsNullOrWhiteSpace(e.PropertyName) ? "Details" : e.PropertyName,
                             e => e.ErrorMessage)
                    .ToDictionary(g => g.Key, g => g.ToArray()));
            }
            else if (exception is Exceptions.ValidationException customValEx)
            {
                details = new ValidationProblemDetails(customValEx.Errors);
            }
            else if (!httpContext.Request.HasFormContentType && httpContext.Items.ContainsKey("ModelStateErrors"))
            {
                // optional place to add model-state aggregation if passed externally
                details = new ProblemDetails();
            }
            else
            {
                details = new ProblemDetails();
            }
        }
        else
        {
            details = new ProblemDetails();
        }

        details.Status = status;
        details.Title = title;
        details.Type = type;
        details.Detail = detail;
        details.Instance = httpContext.Request.Path;

        if (extensions != null)
        {
            foreach (var kv in extensions)
            {
                details.Extensions[kv.Key] = kv.Value;
            }
        }

        // Always add trace id for correlation
        details.Extensions.TryAdd("traceId", httpContext.TraceIdentifier);

        return details;
    }
}
