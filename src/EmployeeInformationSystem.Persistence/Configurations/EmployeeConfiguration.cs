using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employee");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("EmployeeId")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.EmployeeNo)
                .HasMaxLength(30)
                .IsRequired();

            builder.HasIndex(x => x.EmployeeNo)
                .IsUnique();

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

            builder.Property(x => x.CreatedBy)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.HasOne<Department>()
                .WithMany()
                .HasForeignKey(x => x.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Position>()
                .WithMany()
                .HasForeignKey(x => x.PositionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
