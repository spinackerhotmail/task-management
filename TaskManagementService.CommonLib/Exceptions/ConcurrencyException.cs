namespace TaskManagementService.CommonLib.Exceptions;

/// <summary>
/// Исключение для проблем с параллельным доступом
/// </summary>
public class ConcurrencyException : Exception
{
    public ConcurrencyException(string message) : base(message)
    {
    }

    public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
