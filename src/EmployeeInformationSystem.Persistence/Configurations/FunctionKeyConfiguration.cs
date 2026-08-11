using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class FunctionKeyConfiguration : IEntityTypeConfiguration<FunctionKey>
    {
        public void Configure(EntityTypeBuilder<FunctionKey> builder)
        {
            builder.ToTable("FunctionKey");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("FunctionKeyId")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.FunctionCode)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.FunctionCode)
                .IsUnique();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Remarks)
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
        }
    }
}