using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ILicenseTypeRepo
    {
        public Task<LicenseType?> GetByIdAsync(int licenseClassId);
        public Task<LicenseType?> GetByClassNameAsync(string? className);
        public Task<List<LicenseType>?> GetAllLicenseClassesAsync();
        public Task<int> AddLicenseClassAsync(LicenseType? licenseType);
        public Task<bool> UpdateLicenseClassAsync(LicenseType? licenseType);
    }

}
