using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations;

public class UserHistoryConfiguration : IEntityTypeConfiguration<UserHistory>
{
    public void Configure(EntityTypeBuilder<UserHistory> builder)
    {
        builder.ToTable("UserHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("HistoryId")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.UserId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.UserName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.LastLogin)
            .HasColumnType("datetimeoffset(7)");

        builder.Property(x => x.FailedLoginAttempt)
            .IsRequired();

        builder.Property(x => x.PasswordChangedDate)
            .HasColumnType("datetimeoffset(7)");

        builder.Property(x => x.MustChangePassword)
            .IsRequired();

        builder.Property(x => x.IsLocked)
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

        builder.HasIndex(x => x.UserId);
    }
}