using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class DetainedLicensesRepo : BaseRepoHelper, IDetainedRepo
    {
        private readonly AppDbContext _context;

        public DetainedLicensesRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<int> AddDetainedLicenseAsync(DetainedLicense? detainedLicense)
        {
            if (detainedLicense == null)
                throw new ArgumentNullException(nameof(detainedLicense));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.DetainedLicenses.AddAsync(detainedLicense);
                await _context.SaveChangesAsync();
                return detainedLicense.Id;
            });
        }

        public async Task<List<DetainedLicense>?> GetAllDetainedLicensesAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.DetainedLicenses.ToListAsync());
        }

        public async Task<DetainedLicense?> GetByIdAsync(int detainId)
        {
            if (detainId <= 0)
                throw new ArgumentException($"ID {detainId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.DetainedLicenses.FindAsync(detainId));
        }

        public async Task<DetainedLicense?> GetByLicenseIdAsync(int licenseId)
        {
            if (licenseId <= 0)
                throw new ArgumentException($"ID {licenseId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.DetainedLicenses.FindAsync(licenseId));
        }

        public async Task<bool> IsLicenseDetainedAsync(int licenseId)
        {
            return await ExecuteDbOperationAsync(async () => await _context.DetainedLicenses.AnyAsync
                   (x => x.LicenseId == licenseId && !x.IsReleased));
        }

        public async Task<bool> ReleaseDetainedLicenseAsync(int detainId, int releasedByUserId, int releaseAppId)
        {
            if (detainId <= 0)
                throw new ArgumentException($"ID {detainId} must be greater then 0.");

            var detainedLicense = await _context.DetainedLicenses.FindAsync(detainId);

            if(detainedLicense == null)
                throw new ArgumentNullException(nameof(detainedLicense)); 

            detainedLicense.ReleasedByUserId = releasedByUserId;
            detainedLicense.ReleaseAppId = releaseAppId;
            detainedLicense.ReleaseDate = DateTime.UtcNow;

            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> UpdateDetainedLicenseAsync(DetainedLicense? detainedLicense)
        {
            if (detainedLicense == null)
                throw new ArgumentNullException(nameof(detainedLicense));

            _context.DetainedLicenses.Update(detainedLicense);
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }
    }

}
