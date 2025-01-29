using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class InternationalRepoService
    {
        private InternationalRepository _repository;

        public InternationalRepoService()
        {
            _repository = new InternationalRepository();
        }

        public int AddNew(InternationalDTO dto) => _repository.AddNewInternationalLicense(dto);

        public bool Update(InternationalDTO dto) => _repository.UpdateInternationalLicense(dto);

        public InternationalDTO Find(int InternationalLicenseId) =>
            _repository.GetInternationalLicenseById(InternationalLicenseId);

        public DataTable GetAll() => _repository.GetAllInternationalLicenses();

        public int GetLicenseIdByDriverId(int driverId)
            => _repository.GetActiveInternationalLicenseIdByDriverId(driverId);

        public DataTable GetDriverInternationalLicenses(int DriverID)
            => _repository.GetDriverInternationalLicenses(DriverID);


    }

}
