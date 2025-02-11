using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class DrivingLicensesRepo : BaseRepoHelper, ILicenseRepo
    {
        private readonly AppDbContext _context;

        public DrivingLicensesRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<DrivingLicense?> GetByLicenseIdAsync(int licenseId)
        {
            if (licenseId <= 0)
                throw new ArgumentException($"License ID {licenseId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.DrivingLicenses.FindAsync(licenseId));
        }

        public async Task<List<DrivingLicense>?> GetAllLicensesAsync()
        {
            return await ExecuteDbOperationAsync(async () =>
                await _context.DrivingLicenses.ToListAsync());
        }

        public async Task<List<DrivingLicense>?> GetDriverLicensesAsync(int driverId)
        {
            if (driverId <= 0)
                throw new ArgumentException($"Driver ID {driverId} must be greater than 0.");

            const string query = @"
                   SELECT     
                       Licenses.LicenseID,
                       ApplicationID,
                       LicenseClasses.ClassName, 
                       Licenses.IssueDate, 
                       Licenses.ExpirationDate, 
                       Licenses.IsActive
                   FROM Licenses 
                   INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                   WHERE DriverID = @DriverID
                   ORDER BY IsActive DESC, ExpirationDate DESC";

            return await ExecuteDbOperationAsync(async () => await _context.DrivingLicenses.FromSqlRaw(query,
                new SqlParameter("@DriverID", driverId)).OrderByDescending(x => x.IsActive)
                    .ThenByDescending(x => x.ExpirationDate)
                    .ToListAsync());
        }

        public async Task<int> AddLicenseAsync(DrivingLicense? license)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.DrivingLicenses.AddAsync(license);
                await _context.SaveChangesAsync();
                return license.Id;
            });
        }

        public async Task<bool> UpdateLicenseAsync(DrivingLicense? license)
        {
            if (license == null)
                throw new ArgumentNullException(nameof(license));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.DrivingLicenses.Update(license);
                return await _context.SaveChangesAsync() > 0;
            });
        }

        public async Task<int> GetActiveLicenseIdByPersonIdAsync(int personId, int licenseClassId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            if (licenseClassId <= 0)
                throw new ArgumentException($"License Class ID {licenseClassId} must be greater than 0.");

            const string query = @"
                  SELECT Licenses.LicenseID
                  FROM Licenses 
                  INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
                  WHERE 
                      Licenses.LicenseClass = @LicenseClass 
                      AND Drivers.PersonID = @PersonID
                      AND IsActive = 1;";

            return await ExecuteDbOperationAsync(async () => await _context.DrivingLicenses.FromSqlRaw(query,
                new SqlParameter("@LicenseClass", licenseClassId),
                new SqlParameter("@PersonID", personId)).Select(x => x.Id)
                    .FirstOrDefaultAsync());
        }

        public async Task<bool> DeactivateLicenseAsync(int licenseId)
        {
            if (licenseId <= 0)
                throw new ArgumentException($"License ID {licenseId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
            {
                var license = await _context.DrivingLicenses.FindAsync(licenseId);
                if (license == null)
                    throw new ArgumentNullException(nameof(license));

                license.IsActive = false;
                _context.DrivingLicenses.Update(license);
                return await _context.SaveChangesAsync() > 0;
            });
        }

    }
}