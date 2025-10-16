namespace TaskManagementService.CommonLib.Audits;

public class Audit
{
    public long Id { get; set; }
    public string? EntityName { get; set; }
    public string? ActionType { get; set; }
    public string? Username { get; set; }
    public DateTime TimeStamp { get; set; }
    public string? EntityId { get; set; }
    public Dictionary<string, object>? Changes { get; set; }
}
