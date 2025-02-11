using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class InternationalRepo : BaseRepoHelper, IInternationalLicenseRepo
    {
        private readonly AppDbContext _context;

        public InternationalRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<InternationalLicense?> GetByIdAsync(int internationalLicenseId)
        {
            if (internationalLicenseId <= 0)
                throw new ArgumentException($"ID {internationalLicenseId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.InternationalLicenses.FindAsync(internationalLicenseId));
        }

        public async Task<List<InternationalLicense>?> GetAllInternationalLicensesAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.InternationalLicenses.ToListAsync());
        }

        public async Task<List<InternationalLicense>?> GetDriverInternationalLicensesAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException($"Driver ID {driverId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.InternationalLicenses
                    .Where(x => x.DriverId == driverId)
                    .OrderByDescending(x => x.ExpireDate)
                    .ToListAsync());
        }

        public async Task<int> AddInternationalLicenseAsync(InternationalLicense? internationalLicense)
        {
            if (internationalLicense == null)
                throw new ArgumentNullException(nameof(internationalLicense));

            return await ExecuteDbOperationAsync(async () =>
            {
                if (!await DeActivateOldLicense(internationalLicense.DriverId))
                    throw new DeactivatingLicenseFailedException("De-Activating License Failed");

                // Add the new license
                await _context.InternationalLicenses.AddAsync(internationalLicense);
                await _context.SaveChangesAsync();

                return internationalLicense.Id;
            });
        }

        public async Task<bool> UpdateInternationalLicenseAsync(InternationalLicense? internationalLicense)
        {
            if (internationalLicense == null)
                throw new ArgumentNullException(nameof(internationalLicense));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.InternationalLicenses.Update(internationalLicense);
                return await _context.SaveChangesAsync() > 0;
            });
        }

        public async Task<int> GetActiveInternationalLicenseIdByDriverIdAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException($"Driver ID {driverId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.InternationalLicenses
                    .Where(x => x.DriverId == driverId
                               && DateTime.UtcNow >= x.IssueDate
                               && DateTime.UtcNow <= x.ExpireDate)
                    .OrderByDescending(x => x.ExpireDate)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync());
        }


        //Private Helper
        private async Task<bool> DeActivateOldLicense(int driverId)
        {
            // Deactivate existing licenses for the driver
            var existingLicense = await _context.InternationalLicenses
                .FirstOrDefaultAsync(x => x.DriverId == driverId);

            if (existingLicense != null)
            {
                existingLicense.IsActive = false;
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }

    internal class DeactivatingLicenseFailedException : Exception
    {
        public DeactivatingLicenseFailedException(string? message) : base(message)
        {
        }
    }

}
