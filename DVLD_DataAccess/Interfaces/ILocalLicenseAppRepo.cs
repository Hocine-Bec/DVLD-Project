using DVLD_DataAccess.Core.Entities.Applications;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ILocalLicenseAppRepo
    {
        public Task<(int BaseAppId, int licenseTypeId)?> GetByIdAsync(int localLicenseAppId);
        public Task<(int localLicenseAppId, int licenseTypeId)?> GetByAppIdAsync(int appId);
        public Task<List<LocalLicenseApp>?> GetAllLocalLicenseAppAsync();
        public Task<int> AddLocalLicenseAppAsync(int applicationId, int licenseClassId);
        public Task<bool> UpdateLocalLicenseAppAsync(int localLicenseAppId, int applicationId, int licenseClassId);
        public Task<bool> DeleteLocalLicenseAppAsync(int localLicenseAppId);
    }
}
