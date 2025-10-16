using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace TaskManagementService.CommonLib.Audits;

public class AuditEntry
{
    public Audit Audit { get; set; }

    // TempProperties are used for properties that are only generated on save, e.g. ID's
    public List<PropertyEntry> TempProperties { get; set; }
}
