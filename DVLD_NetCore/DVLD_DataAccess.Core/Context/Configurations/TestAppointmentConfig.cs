using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class TestAppointmentConfig : IEntityTypeConfiguration<TestAppointment>
    {
        public void Configure(EntityTypeBuilder<TestAppointment> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("TestAppointments");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Date configuration
            entity.Property(t => t.AppointmentDate)
                  .HasColumnType("datetime")
                  .IsRequired();

            // Fee configuration
            entity.Property(t => t.PaidFees)
                  .HasColumnType("smallmoney")
                  .IsRequired();

            // Status configuration
            entity.Property(t => t.IsLocked)
                  .HasColumnType("bit")
                  .HasDefaultValue(false);

            // Foreign key relationships
            entity.HasOne(x => x.Test)
                 .WithOne(x => x.TestAppointment)
                  .HasForeignKey<TestAppointment>(x => x.Id)
                 .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<TestType>()
                  .WithMany()
                  .HasForeignKey(t => t.TestTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<LocalLicenseApp>()
                  .WithMany()
                  .HasForeignKey(t => t.LocalLicenseAppId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<BaseApp>()
                  .WithMany()
                  .HasForeignKey(t => t.RetakeTestAppId)
                  .OnDelete(DeleteBehavior.Restrict)
                  .IsRequired(false);
        }
    }
}
