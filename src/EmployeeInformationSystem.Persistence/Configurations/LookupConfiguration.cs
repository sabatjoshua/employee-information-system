using EmployeeInformationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeInformationSystem.Persistence.Configurations
{
    public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            builder.ToTable("Lookup");

            builder.HasKey(x => x.LookupId);

            builder.Property(x => x.LookupId)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Type)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Code)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Sort)
                .IsRequired();

            builder.HasIndex(x => new { x.Type, x.Code })
                .IsUnique();
        }
    }
}