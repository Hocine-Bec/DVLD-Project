using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class LocalLicenseConfig : IEntityTypeConfiguration<LocalLicenseApp>
    {
        public void Configure(EntityTypeBuilder<LocalLicenseApp> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("LocalDrivingLicenseApplications");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Foreign key relationships
            entity.HasOne<BaseApp>()
                  .WithMany()
                  .HasForeignKey(l => l.BaseAppId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<LicenseType>()
                  .WithMany()
                  .HasForeignKey(l => l.LicenseTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
