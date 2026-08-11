using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations;

public class EmployeeHistoryConfiguration : IEntityTypeConfiguration<EmployeeHistory>
{
    public void Configure(EntityTypeBuilder<EmployeeHistory> builder)
    {
        builder.ToTable("EmployeeHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("HistoryId")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.EmployeeNo)
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.MiddleName)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(x => x.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.BirthDate)
            .IsRequired();

        builder.Property(x => x.HireDate)
            .IsRequired();

        builder.Property(x => x.GenderCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.Email)
            .HasMaxLength(255);

        builder.Property(x => x.MobileNo)
            .HasMaxLength(30);

        builder.Property(x => x.StatusCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ActionTypeCode)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.ActionBy)
            .IsRequired();

        builder.Property(x => x.ActionAt)
            .IsRequired();

        builder.Property(x => x.CreatedBy)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => x.EmployeeId);
    }
}