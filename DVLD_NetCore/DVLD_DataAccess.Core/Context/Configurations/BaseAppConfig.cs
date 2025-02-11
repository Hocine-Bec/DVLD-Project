using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class BaseAppConfig
    {
        public void Configure(EntityTypeBuilder<BaseApp> entity)
        {
            entity.Property(e => e.CreatedAt)
               .HasColumnType("datetime")
               .HasDefaultValueSql("GETDATE()")
               .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime");

            // Table name
            entity.ToTable("Applications");

            // Primary Key
            entity.HasKey(x => x.Id);

            // Properties
            entity.Property(x => x.Id)
                .HasColumnName("ApplicationID")
                .HasColumnType("int")
                .ValueGeneratedOnAdd(); // Assuming the database generates the ID

            entity.Property(x => x.PersonId)
                .HasColumnName("ApplicantPersonID")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(x => x.AppDate)
                .HasColumnName("ApplicationDate")
                .HasColumnType("datetime")
                .IsRequired()
                .HasDefaultValueSql("GETDATE()"); // Default to current date and time

            entity.Property(x => x.AppTypeId)
                .HasColumnName("ApplicationTypeID")
                .HasColumnType("int")
                .IsRequired();

            entity.Property(x => x.Status)
                 .HasConversion<int>()
                 .HasColumnType("tinyint")
                 .IsRequired();

            entity.Property(x => x.LastStatusDate)
                .HasColumnName("LastStatusDate")
                .HasColumnType("datetime");

            entity.Property(x => x.PaidFees)
                .HasColumnName("PaidFees")
                .HasColumnType("smallmoney");

            entity.Property(x => x.UserId)
                .HasColumnName("CreatedByUserID")
                .HasColumnType("int")
                .IsRequired();

            // Foreign Key Relationships
            entity.HasOne<Person>()
                .WithMany()
                .HasForeignKey(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            entity.HasOne<AppType>()
                .WithMany()
                .HasForeignKey(x => x.AppTypeId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed

            entity.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Adjust delete behavior as needed
        }
    }
}
