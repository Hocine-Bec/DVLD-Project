using DVLD_DataAccess.Core.Entities.Identity;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IDriverRepo
    {
        public Task<Driver?> GetByDriverIdAsync(int driverId);
        public Task<Driver?> GetByPersonIdAsync(int personId);
        public Task<List<Driver>?> GetAllDriversAsync();
        public Task<int> AddDriverAsync(int personId, int userId);
        public Task<bool> UpdateDriverAsync(Driver? driver);
    }

}
