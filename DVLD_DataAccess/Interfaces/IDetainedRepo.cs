using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IDetainedRepo
    {
        public Task<DetainedLicense?> GetByIdAsync(int detainId);
        public Task<DetainedLicense?> GetByLicenseIdAsync(int licenseId);
        public Task<List<DetainedLicense>?> GetAllDetainedLicensesAsync();
        public Task<int> AddDetainedLicenseAsync(DetainedLicense? detainedLicense);
        public Task<bool> UpdateDetainedLicenseAsync(DetainedLicense? detainedLicense);
        public Task<bool> ReleaseDetainedLicenseAsync(int detainId, int releasedByUserId, int releaseAppId);
        public Task<bool> IsLicenseDetainedAsync(int licenseId);
    }

}
