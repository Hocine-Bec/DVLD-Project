using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Identity;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class DriversRepo : BaseRepoHelper, IDriverRepo
    {
        private readonly AppDbContext _context;

        public DriversRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<int> AddDriverAsync(int personId, int userId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            if (userId <= 0)
                throw new ArgumentException($"User ID {userId} must be greater than 0.");

            var driver = new Driver
            {
                PersonId = personId,
                UserId = userId,
                CreatedDate = DateTime.UtcNow
            };

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.Drivers.AddAsync(driver);
                await _context.SaveChangesAsync();
                return driver.Id;
            });
        }

        public async Task<List<Driver>?> GetAllDriversAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.Drivers.ToListAsync());
        }

        public async Task<Driver?> GetByDriverIdAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException($"Driver ID {driverId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Drivers.FindAsync(driverId));
        }

        public async Task<Driver?> GetByPersonIdAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.Drivers.FirstOrDefaultAsync(d => d.PersonId == personId));
        }

        public async Task<bool> UpdateDriverAsync(Driver? driver)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.Drivers.Update(driver);
                return await _context.SaveChangesAsync() > 0;
            });
        }
    }

}
