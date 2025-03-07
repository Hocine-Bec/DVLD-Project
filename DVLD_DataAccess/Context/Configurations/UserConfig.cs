using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class UserConfig
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();

            entity.ToTable("Users");

            entity.HasKey(x => x.Id);

            entity.HasIndex(x => x.PersonId)
                .IsUnique();

            entity.Property(x => x.Id)
                .IsRequired()
                .HasColumnType("int");

            entity.Property(x => x.PersonId)
                .IsRequired()
                .HasColumnType("int");

            entity.Property(x => x.Username)
                .IsRequired()
                .HasColumnType("nvarchar(50)")
                .HasMaxLength(50);

            entity.Property(x => x.Password)
                .IsRequired()
                .HasColumnType("nvarchar(20)")
                .HasMaxLength(20);

            entity.Property(x => x.IsActive)
                .IsRequired()
                .HasColumnType("bit")
                .HasDefaultValue(true);

            entity.HasOne<Person>()
                  .WithOne()
                  .HasForeignKey<User>(x => x.PersonId)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }

}
