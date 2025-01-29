using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class DetainedRepoService
    {
        private DetainedRepository _repository;

        public DetainedRepoService()
        {
            _repository = new DetainedRepository();
        }

        public int AddNew(DetainedDTO dto) => _repository.AddNewDetainedLicense(dto);

        public bool Update(DetainedDTO dto) => _repository.UpdateDetainedLicense(dto);

        public DetainedDTO FindById(int detainId) =>
            _repository.GetDetainedLicenseInfoById(detainId);

        public DetainedDTO FindByLicenseId(int licenseId) 
            => _repository.GetDetainedLicenseInfoByLicenseId(licenseId);

        public DataTable GetAllDetained() => _repository.GetAllDetainedLicenses();

        public bool IsDetained(int LicenseID) => _repository.IsLicenseDetained(LicenseID);

        public bool Release(int detainId, int releasedByUserId, int releaseAppId)
            => _repository.ReleaseDetainedLicense(detainId, releasedByUserId, releaseAppId);
    }

}
