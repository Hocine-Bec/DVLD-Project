using DVLD_DataAccess.Core.Entities.Applications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class AppTypeConfig : IEntityTypeConfiguration<AppType>
    {
        public void Configure(EntityTypeBuilder<AppType> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("ApplicationTypes");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Title configuration
            entity.HasIndex(a => a.Title)
                  .IsUnique();

            entity.Property(a => a.Title)
                  .HasMaxLength(150)
                  .IsRequired()
                  .HasColumnType("nvarchar(150)");

            // Fee configuration
            entity.Property(a => a.Fees)
                  .HasColumnType("smallmoney")
                  .IsRequired();
        }
    }
}
