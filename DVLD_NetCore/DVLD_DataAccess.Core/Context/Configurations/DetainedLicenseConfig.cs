using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class DetainedLicenseConfig : IEntityTypeConfiguration<DetainedLicense>
    {
        public void Configure(EntityTypeBuilder<DetainedLicense> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("DetailedLicenses");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Date configurations
            entity.Property(d => d.DetainDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(d => d.ReleaseDate)
                  .HasColumnType("datetime");

            // Fee configuration
            entity.Property(d => d.FineFees)
                  .HasColumnType("smallmoney")
                  .IsRequired();

            // Status configuration
            entity.Property(d => d.IsReleased)
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            // Foreign key relationships
            entity.HasOne<DrivingLicense>()
                  .WithMany()
                  .HasForeignKey(d => d.LicenseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(d => d.CreatedByUserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(d => d.ReleasedByUserId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);

            entity.HasOne<BaseApp>()
                  .WithMany()
                  .HasForeignKey(d => d.ReleaseAppId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        }
    }
}
