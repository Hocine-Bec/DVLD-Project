using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace DVLD_DataAccess.Core.Context
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Person> People { get; set; }

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
