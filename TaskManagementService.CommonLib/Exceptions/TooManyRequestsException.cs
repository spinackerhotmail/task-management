namespace TaskManagementService.CommonLib.Exceptions;

/// <summary>
/// Исключение для случаев превышения лимита запросов (Rate Limiting)
/// </summary>
public class TooManyRequestsException : Exception
{
    public TimeSpan? RetryAfter { get; }

    public TooManyRequestsException(string message) : base(message)
    {
    }

    public TooManyRequestsException(string message, TimeSpan retryAfter) : base(message)
    {
        RetryAfter = retryAfter;
    }

    public TooManyRequestsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
