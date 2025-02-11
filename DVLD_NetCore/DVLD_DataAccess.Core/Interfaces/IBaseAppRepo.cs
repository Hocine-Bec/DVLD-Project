using DVLD_DataAccess.Core.Entities.Applications;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IBaseAppRepo
    {
        public Task<int> AddBaseAppAsync(BaseApp? baseApp);
        public Task<bool> DeleteBaseAppAsync(int appId);
        public Task<bool> DoesPersonHaveActiveBaseAppAsync(int personId, int appTypeId);
        public Task<int> GetActiveAppIdByLicenseTypeAsync(int personId, int appTypeId, int licenseTypeId);
        public Task<int> GetActiveBaseAppIdAsync(int personId, int appTypeId);
        public Task<List<BaseApp>?> GetAllBaseAppsAsync();
        public Task<BaseApp?> GetByBaseAppIdIdAsync(int appId);
        public Task<bool> DoesBaseAppExistAsync(int appId);
        public Task<bool> UpdateBaseAppAsync(BaseApp? baseApp);
        public Task<bool> UpdateStatusAsync(int appId, short newStatus);
    }
}
