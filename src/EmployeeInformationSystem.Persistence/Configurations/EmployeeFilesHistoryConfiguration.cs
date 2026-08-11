using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations;

public class EmployeeFilesHistoryConfiguration : IEntityTypeConfiguration<EmployeeFilesHistory>
{
    public void Configure(EntityTypeBuilder<EmployeeFilesHistory> builder)
    {
        builder.ToTable("EmployeeFilesHistory");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("HistoryId")
            .ValueGeneratedOnAdd();

        builder.Property(x => x.EmployeeFileId)
            .IsRequired();

        builder.Property(x => x.EmployeeId)
            .IsRequired();

        builder.Property(x => x.FilePath)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.OriginalFileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.StoredFileName)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Extension)
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.FileSize)
            .IsRequired();

        builder.Property(x => x.ContentType)
            .HasMaxLength(100);

        builder.Property(x => x.StorageType)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.StoragePath)
            .HasMaxLength(500);

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

        builder.HasIndex(x => x.EmployeeFileId);
    }
}