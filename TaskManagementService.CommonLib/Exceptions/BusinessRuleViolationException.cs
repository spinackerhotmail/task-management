namespace TaskManagementService.CommonLib.Exceptions;

/// <summary>
/// Исключение для критических бизнес-ошибок
/// </summary>
public class BusinessRuleViolationException : Exception
{
    public string? RuleCode { get; }

    public BusinessRuleViolationException(string message) : base(message)
    {
    }

    public BusinessRuleViolationException(string message, string ruleCode) : base(message)
    {
        RuleCode = ruleCode;
    }

    public BusinessRuleViolationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
