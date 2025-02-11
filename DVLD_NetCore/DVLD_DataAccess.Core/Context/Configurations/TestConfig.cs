using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class TestConfig : IEntityTypeConfiguration<Test>
    {
        public void Configure(EntityTypeBuilder<Test> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("Tests");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Result configuration
            entity.Property(t => t.TestResult)
                  .HasColumnType("bit");

            // Notes configuration
            entity.Property(t => t.Notes)
                  .HasMaxLength(500)
                  .HasColumnType("nvarchar(500)");

            // Foreign key relationships
            entity.HasOne(x => x.TestAppointment)
                .WithOne(x => x.Test)
                .HasForeignKey<Test>(x => x.TestAppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne<User>()
                  .WithMany()
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
