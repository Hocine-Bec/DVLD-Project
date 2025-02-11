using DVLD_DataAccess.Core.Entities.Applications;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class InternationalLicenseConfig : IEntityTypeConfiguration<InternationalLicense>
    {
        public void Configure(EntityTypeBuilder<InternationalLicense> entity)
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

            entity.Property(d => d.IssueDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(d => d.ExpireDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            entity.Property(d => d.IsActive)
                 .HasColumnType("bit")
                 .IsRequired();


            // Foreign key relationships
            entity.HasOne<DrivingLicense>()
                  .WithMany()
                  .HasForeignKey(d => d.IssuedUsingLicenseId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(d => d.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BaseApp>()
                  .WithMany()
                  .HasForeignKey(d => d.BaseAppId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        }

    }
}
