using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Enums;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class BaseAppRepo : BaseRepoHelper, IBaseAppRepo
    {
        private readonly AppDbContext _context;

        public BaseAppRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<int> AddBaseAppAsync(BaseApp? baseApp)
        {
            if(baseApp == null) 
                throw new ArgumentNullException(nameof(baseApp));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.BaseApps.AddAsync(baseApp);
                await _context.SaveChangesAsync();
                return baseApp.Id;
            });
        }

        public async Task<bool> DeleteBaseAppAsync(int appId)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater then 0.");

            var baseApp = await _context.BaseApps.FindAsync(appId);

            if (baseApp is null)
                throw new ArgumentNullException(nameof(baseApp));

            _context.BaseApps.Remove(baseApp);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesBaseAppExistAsync(int appId)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater then 0.");

            return await ExecuteDbOperationAsync(async () => await _context.BaseApps.AnyAsync(x => x.Id == appId));
        }

        public async Task<bool> DoesPersonHaveActiveBaseAppAsync(int personId, int appTypeId)
        {
            if (appTypeId <= 0)
                throw new ArgumentException($"Application Type ID {appTypeId} must be greater than 0.");

            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            int id = await GetActiveBaseAppIdAsync(personId, appTypeId);
            return id != -1;
        }

        public async Task<int> GetActiveAppIdByLicenseTypeAsync(int personId, int appTypeId, int licenseTypeId)
        {
            const string query = @"
            SELECT Applications.ApplicationID  
            FROM Applications 
            INNER JOIN LocalDrivingLicenseApplications 
                ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
            WHERE ApplicantPersonID = @ApplicantPersonID 
                AND ApplicationTypeID = @ApplicationTypeID 
                AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                AND ApplicationStatus = 1";

            var appId = await _context.BaseApps
                .FromSqlRaw(query, 
                     new SqlParameter("@ApplicantPersonID", personId), 
                     new SqlParameter("@ApplicationTypeID", appTypeId), 
                     new SqlParameter("@LicenseClassID", licenseTypeId))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();

            return appId;
        }

        public async Task<int> GetActiveBaseAppIdAsync(int personId, int appTypeId)
        {
            if (appTypeId <= 0)
                throw new ArgumentException($"Application Type ID {appTypeId} must be greater then 0.");

            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater then 0.");

            var baseAppId = await _context.BaseApps.Where(x => x.PersonId == personId
                                      && x.AppTypeId == appTypeId
                                      && (int)x.Status == 1).Select(x => x.Id).FirstOrDefaultAsync();

            if (baseAppId <= 0)
                throw new ArgumentNullException($"Application Type ID {baseAppId} must be greater then 0.");

            return baseAppId;
        }

        public async Task<List<BaseApp>?> GetAllBaseAppsAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.BaseApps.ToListAsync());
        }

        public async Task<BaseApp?> GetByBaseAppIdIdAsync(int appId)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater then 0.");

            return await ExecuteDbOperationAsync(async () => await _context.BaseApps.FindAsync(appId));
        }

        public async Task<bool> UpdateBaseAppAsync(BaseApp? baseApp)
        {
            if(baseApp == null)
                throw new ArgumentNullException(nameof(baseApp));

            _context.BaseApps.Update(baseApp);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStatusAsync(int appId, short newStatus)
        {
            if (appId <= 0)
                throw new ArgumentException($"Application ID {appId} must be greater then 0.");

            if (newStatus <= 0)
                throw new ArgumentException($"New Status {newStatus} must be greater then 0.");

            var baseApp = await _context.BaseApps.FindAsync(appId);

            if (baseApp == null)
                throw new ArgumentNullException(nameof(baseApp));

            baseApp.Status = (AppStatus)newStatus;
            _context.BaseApps.Update(baseApp);
            return await _context.SaveChangesAsync() > 0;
        }
    }

}
