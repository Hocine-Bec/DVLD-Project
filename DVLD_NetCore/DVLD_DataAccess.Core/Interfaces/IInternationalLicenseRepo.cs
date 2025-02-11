using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Repositories;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IInternationalLicenseRepo
    {
        public Task<InternationalLicense?> GetByIdAsync(int internationalLicenseId);
        public Task<List<InternationalLicense>?> GetAllInternationalLicensesAsync();
        public Task<List<InternationalLicense>?> GetDriverInternationalLicensesAsync(int driverId);
        public Task<int> AddInternationalLicenseAsync(InternationalLicense? internationalLicense);
        public Task<bool> UpdateInternationalLicenseAsync(InternationalLicense? internationalLicense);
        public Task<int> GetActiveInternationalLicenseIdByDriverIdAsync(int driverId);
    }

}
