using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class LicenseTypeConfig : IEntityTypeConfiguration<LicenseType>
    {
        public void Configure(EntityTypeBuilder<LicenseType> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("LicenseClasses");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Name configuration
            entity.HasIndex(l => l.ClassName)
                  .IsUnique();

            entity.Property(l => l.ClassName)
                  .HasMaxLength(50)
                  .IsRequired()
                  .HasColumnType("nvarchar(50)");

            // Description configuration
            entity.Property(l => l.ClassDescription)
                  .HasMaxLength(500)
                  .HasColumnType("nvarchar(500)");

            // Age and validity configuration
            entity.Property(l => l.MinimumAllowedAge)
                  .HasColumnType("tinyint")
                  .IsRequired();

            entity.Property(l => l.DefaultValidityLength)
                  .HasColumnType("tinyint")
                  .IsRequired();

            // Fee configuration
            entity.Property(l => l.ClassFees)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();
        }
    }
}
