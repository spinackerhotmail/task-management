using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagementService.CommonLib.Audits;
using Newtonsoft.Json;

namespace TaskManagementService.CommonLib.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
    public void Configure(EntityTypeBuilder<Audit> builder)
    {
        builder
               .Property((Audit ae) => ae.Changes)
               .HasConversion((Dictionary<string, object> value) => JsonConvert.SerializeObject(value),
               (string serializedValue) => JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedValue));
    }
}
