namespace TaskManagementService.CommonLib.Exceptions;

/// <summary>
/// Исключение для недоступности сервиса
/// </summary>
public class ServiceUnavailableException : Exception
{
    public TimeSpan? RetryAfter { get; }

    public ServiceUnavailableException(string message) : base(message)
    {
    }

    public ServiceUnavailableException(string message, TimeSpan retryAfter) : base(message)
    {
        RetryAfter = retryAfter;
    }

    public ServiceUnavailableException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
