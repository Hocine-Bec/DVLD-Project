using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Context.Configuration
{
    public class PersonConfig : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> entity)
        {

            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table configuration
            entity.ToTable("People");  

            // Primary key configuration
            entity.HasKey(e => e.Id);

            // National Number configuration
            entity.HasIndex(p => p.NationalNo)
                  .IsUnique();  
            entity.Property(p => p.NationalNo)
                  .HasMaxLength(20)
                  .IsRequired()
                  .HasColumnType("nvarchar(20)");  

            // Name properties configuration
            entity.Property(x => x.FirstName)
                  .HasMaxLength(20)  
                  .IsRequired()
                  .HasColumnType("nvarchar(20)");

            entity.Property(x => x.SecondName)
                  .HasMaxLength(20)
                  .HasColumnType("nvarchar(20)");

            entity.Property(x => x.ThirdName)
                  .HasMaxLength(20)
                  .HasColumnType("nvarchar(20)");

            entity.Property(x => x.LastName)
                  .HasMaxLength(20)
                  .IsRequired()
                  .HasColumnType("nvarchar(20)");

            // Contact information configuration
            entity.Property(x => x.Address)
                  .HasMaxLength(500)
                  .HasColumnType("nvarchar(500)");

            entity.Property(x => x.Phone)
                  .HasMaxLength(20)
                  .HasColumnType("nvarchar(20)");

            entity.Property(x => x.Email)
                  .HasMaxLength(50)
                  .HasColumnType("nvarchar(50)");

            // Image path configuration
            entity.Property(x => x.ImagePath)
                  .HasMaxLength(250)
                  .HasColumnType("nvarchar(250)");

            // Gender configuration
            entity.Property(x => x.Gender)
                  .HasConversion<int>()
                  .HasColumnType("tinyint");

            // Foreign key relationship 
            entity.HasOne<Country>()  
                  .WithMany()
                  .HasForeignKey(x => x.CountryId)
                  .OnDelete(DeleteBehavior.Restrict);  
        }
    }

}
