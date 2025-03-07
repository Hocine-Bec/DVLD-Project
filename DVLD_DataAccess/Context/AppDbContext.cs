using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Entities.Base;
using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess.Core.Context
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BaseApp> BaseApps { get; set; }
        public virtual DbSet<Country> Countries { get; set; }
        public virtual DbSet<DrivingLicense> DrivingLicenses { get; set; }
        public virtual DbSet<BaseApp> Applications { get; set; }
        public virtual DbSet<TestType> TestTypes { get; set; }
        public virtual DbSet<Test> Tests { get; set; }
        public virtual DbSet<TestAppointment> TestAppointments { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<LicenseType> LicenseTypes { get; set; }
        public virtual DbSet<DetainedLicense> DetainedLicenses { get; set; }
        public virtual DbSet<LocalLicenseApp> LocalLicenses { get; set; }
        public virtual DbSet<AppType> ApplicationTypes { get; set; }
        public virtual DbSet<InternationalLicense> InternationalLicenses { get; set; }

        public AppDbContext() { } 
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {  
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

    }
}
