using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class LocalLicenseAppRepo : BaseRepoHelper, ILocalLicenseAppRepo
    {
        private readonly AppDbContext _context;

        public LocalLicenseAppRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<(int BaseAppId, int licenseTypeId)?> GetByIdAsync(int localLicenseAppId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var app = await _context.LocalLicenses
                    .Where(x => x.Id == localLicenseAppId)
                    .Select(x => new { x.BaseAppId, x.LicenseTypeId })
                    .FirstOrDefaultAsync();

                return app != null ? (app.BaseAppId, app.LicenseTypeId) : ((int, int)?)null;
            });
        }

        public async Task<(int localLicenseAppId, int licenseTypeId)?> GetByAppIdAsync(int appId)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var app = await _context.LocalLicenses
                    .Where(x => x.BaseAppId == appId)
                    .Select(x => new { x.Id, x.LicenseTypeId })
                    .FirstOrDefaultAsync();

                return app != null ? (app.Id, app.LicenseTypeId) : ((int, int)?)null;
            });
        }

        public async Task<int> AddLocalLicenseAppAsync(int appId, int licenseTypeId)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater than 0.");

            if (licenseTypeId <= 0)
                throw new ArgumentException($"License Class ID {licenseTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var localLicenseApp = new LocalLicenseApp
                {
                    BaseAppId = appId,
                    LicenseTypeId = licenseTypeId
                };

                await _context.LocalLicenses.AddAsync(localLicenseApp);
                await _context.SaveChangesAsync();

                return localLicenseApp.Id;
            });
        }

        public async Task<bool> UpdateLocalLicenseAppAsync(int localLicenseAppId, int appId, int licenseTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater than 0.");

            if (licenseTypeId <= 0)
                throw new ArgumentException($"License Class ID {licenseTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var localLicenseApp = await _context.LocalLicenses
                    .FirstOrDefaultAsync(x => x.Id == localLicenseAppId);

                if (localLicenseApp == null)
                    throw new ArgumentNullException(nameof(localLicenseApp));

                localLicenseApp.BaseAppId = appId;
                localLicenseApp.LicenseTypeId = licenseTypeId;

                _context.LocalLicenses.Update(localLicenseApp);
                return await _context.SaveChangesAsync() > 0;
            });
        }

        public async Task<bool> DeleteLocalLicenseAppAsync(int localLicenseAppId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var localLicenseApp = await _context.LocalLicenses
                    .FirstOrDefaultAsync(x => x.Id == localLicenseAppId);

                if (localLicenseApp == null)
                    throw new ArgumentNullException(nameof(localLicenseApp));

                _context.LocalLicenses.Remove(localLicenseApp);
                return await _context.SaveChangesAsync() > 0;
            });
        }

        public async Task<List<LocalLicenseApp>?> GetAllLocalLicenseAppAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.LocalLicenses.ToListAsync());
        }

    }
}