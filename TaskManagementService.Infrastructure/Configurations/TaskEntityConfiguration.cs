using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskManagementService.Domain.Entities;
using TaskManagementService.Domain.Enums;

namespace TaskManagementService.Infrastructure.Configurations;

public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
{
    public void Configure(EntityTypeBuilder<TaskEntity> builder)
    {
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(d => d.Status)
            .HasConversion(new EnumToStringConverter<TaskEntityStatus>());

        builder
            .HasIndex(c => new { c.Id });
    }
}
