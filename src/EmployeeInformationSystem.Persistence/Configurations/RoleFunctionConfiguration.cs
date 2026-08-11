using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class RoleFunctionConfiguration : IEntityTypeConfiguration<RoleFunction>
    {
        public void Configure(EntityTypeBuilder<RoleFunction> builder)
        {
            builder.ToTable("RoleFunction");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("RoleFunctionId")
                .ValueGeneratedOnAdd();

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

            builder.HasOne<Role>()
                .WithMany()
                .HasForeignKey(x => x.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<FunctionKey>()
                .WithMany()
                .HasForeignKey(x => x.FunctionKeyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => new { x.RoleId, x.FunctionKeyId })
                .IsUnique();
        }
    }
}