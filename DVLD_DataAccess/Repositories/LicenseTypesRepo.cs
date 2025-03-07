using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class LicenseTypesRepository : BaseRepoHelper, ILicenseTypeRepo
    {
        private readonly AppDbContext _context;

        public LicenseTypesRepository(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<LicenseType?> GetByIdAsync(int licenseClassId)
        {
            if (licenseClassId <= 0)
                throw new ArgumentException($"License Class ID {licenseClassId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.LicenseTypes.FindAsync(licenseClassId));
        }

        public async Task<LicenseType?> GetByClassNameAsync(string? className)
        {
            if (string.IsNullOrWhiteSpace(className))
                throw new ArgumentException("Class name cannot be null or empty.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.LicenseTypes.FirstOrDefaultAsync(x => x.ClassName == className));
        }

        public async Task<List<LicenseType>?> GetAllLicenseClassesAsync()
        {
            return await ExecuteDbOperationAsync(async () =>
                await _context.LicenseTypes
                    .OrderBy(x => x.ClassName)
                    .ToListAsync());
        }

        public async Task<int> AddLicenseClassAsync(LicenseType? licenseClass)
        {
            if (licenseClass == null)
                throw new ArgumentNullException(nameof(licenseClass));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.LicenseTypes.AddAsync(licenseClass);
                await _context.SaveChangesAsync();
                return licenseClass.Id;
            });
        }

        public async Task<bool> UpdateLicenseClassAsync(LicenseType? licenseClass)
        {
            if (licenseClass == null)
                throw new ArgumentNullException(nameof(licenseClass));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.LicenseTypes.Update(licenseClass);
                return await _context.SaveChangesAsync() > 0;
            });
        }
    }
}