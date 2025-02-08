using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Context.Configurations
{
    public class CountryConfig
    {
        public void Configure(EntityTypeBuilder<Country> entity)
        {
            entity.ToTable("Countries");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).IsRequired();

            entity.Property(x => x.CountryName)
                    .HasMaxLength(50)
                    .IsRequired()
                    .HasColumnType("nvarchar(50)");
        }
    }
}
