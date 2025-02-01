using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class LicenseRepoService
    {
        private LicenseRepository _repository;

        public LicenseRepoService()
        {
            _repository = new LicenseRepository();
        }

        public LicenseDTO Find(int licenseId) => _repository.GetLicenseInfoById(licenseId);

        public int AddNewLicense(LicenseDTO dto) => _repository.AddNewLicense(dto);

        public bool UpdateLicense(LicenseDTO dto) => _repository.UpdateLicense(dto);

        public int GetActiveLicenseIdByPersonId(int personID, int licenseClassId)
            => _repository.GetActiveLicenseIdByPersonId(personID, licenseClassId);

        public DataTable GetAllLicenses() => _repository.GetAllLicenses();

        public DataTable GetDriverLicenses(int driverId) => _repository.GetDriverLicenses(driverId);

        public bool DeactivateCurrentLicense(int licenseId) => _repository.DeactivateLicense(licenseId);
    }

    
}
