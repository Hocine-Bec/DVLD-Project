using DVLD_DataAccess.Core.Entities;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ILicenseRepo
    {
        public Task<DrivingLicense?> GetByLicenseIdAsync(int licenseId);
        public Task<List<DrivingLicense>?> GetAllLicensesAsync();
        public Task<List<DrivingLicense>?> GetDriverLicensesAsync(int driverId);
        public Task<int> AddLicenseAsync(DrivingLicense? license);
        public Task<bool> UpdateLicenseAsync(DrivingLicense? license);
        public Task<int> GetActiveLicenseIdByPersonIdAsync(int personId, int licenseClassId);
        public Task<bool> DeactivateLicenseAsync(int licenseId);
    }

}
