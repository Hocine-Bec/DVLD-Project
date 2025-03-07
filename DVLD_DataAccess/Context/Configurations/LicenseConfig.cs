using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class LicenseConfig : IEntityTypeConfiguration<DrivingLicense>
    {
        public void Configure(EntityTypeBuilder<DrivingLicense> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("Licenses");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Date configurations
            entity.Property(e => e.IssueDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(e => e.ExpirationDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            // Notes configuration
            entity.Property(e => e.Notes)
                  .HasMaxLength(500)
                  .HasColumnType("nvarchar(500)");

            // Fee configuration
            entity.Property(e => e.PaidFees)
                  .HasColumnType("smallmoney")
                  .IsRequired();

            // Status configurations
            entity.Property(e => e.IsActive)
                  .HasColumnType("bit")
                  .HasDefaultValue(true);

            entity.Property(e => e.IssueReason)
                  .HasColumnType("tinyint");

            // Foreign key relationships
            entity.HasOne<BaseApp>()
                  .WithMany()
                  .HasForeignKey(x => x.BaseAppId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<Driver>()
                  .WithMany()
                  .HasForeignKey(x => x.DriverId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<LicenseType>()
                  .WithMany()
                  .HasForeignKey(x => x.LicenseTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(x => x.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
