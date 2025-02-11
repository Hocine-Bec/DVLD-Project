using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class TestTypeConfig : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> entity)
        {
            // Audit fields
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("TestTypes");

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // Title configuration
            entity.HasIndex(t => t.Title)
                  .IsUnique();

            entity.Property(t => t.Title)
                  .HasMaxLength(100)
                  .IsRequired()
                  .HasColumnType("nvarchar(100)");

            // Description configuration
            entity.Property(t => t.Description)
                  .HasMaxLength(500)
                  .HasColumnType("nvarchar(500)");

            // Fee configuration
            entity.Property(t => t.Fees)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();
        }
    }
}
