using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("DepartmentId")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

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