using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations;

public class EmployeeRoleHistoryConfiguration : IEntityTypeConfiguration<EmployeeRoleHistory>
{
    public void Configure(EntityTypeBuilder<EmployeeRoleHistory> builder)
    {
        builder.ToTable("EmployeeRoleHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("HistoryId")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeRoleId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.RoleId)
            .IsRequired();

        builder.Property(x => x.StatusCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ActionTypeCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ActionBy)
            .IsRequired();

        builder.Property(x => x.ActionAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("datetimeoffset(7)")
            .IsRequired();

        builder.HasIndex(x => x.EmployeeRoleId);
    }
}