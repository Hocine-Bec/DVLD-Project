using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DVLD_DataAccess.Core.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {   }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasIndex(p => p.NationalNo)
                .IsUnique();

                entity.Property(p => p.NationalNo)
                .HasMaxLength(20);

                entity.Property(x => x.FirstName)
                .HasMaxLength(50)
                .IsRequired();

                entity.Property(x => x.SecondName).HasMaxLength(50);
                entity.Property(x => x.ThirdName).HasMaxLength(50);
                
                entity.Property(x => x.LastName)
                .HasMaxLength(50)
                .IsRequired();


                entity.Property(x => x.Gender)
                      .HasConversion<int>();
            });
        }
    }
}
