using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class EmployeeFilesConfiguration : IEntityTypeConfiguration<EmployeeFiles>
    {
        public void Configure(EntityTypeBuilder<EmployeeFiles> builder)
        {
            builder.ToTable("EmployeeFiles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("EmployeeFileId")
                .ValueGeneratedOnAdd();

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
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.StorageType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.StoragePath)
                .HasMaxLength(500)
                .IsRequired(false);

            builder.Property(x => x.StatusCode)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .HasColumnType("datetimeoffset(7)")
                .IsRequired();

            builder.Property(x => x.UpdatedBy);

            builder.Property(x => x.UpdatedAt)
                .HasColumnType("datetimeoffset(7)");

            builder.HasOne<Employee>()
                .WithMany()
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}