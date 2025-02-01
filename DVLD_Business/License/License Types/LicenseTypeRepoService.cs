using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class LicenseTypeRepoService
    {
        private LicenseClassRepository _repository;

        public LicenseTypeRepoService()
        {
            _repository = new LicenseClassRepository();
        }

        public int AddNew(LicenseClassDTO dto) => _repository.AddNewLicenseClass(dto);

        public bool Update(LicenseClassDTO dto) => _repository.UpdateLicenseClass(dto);

        public LicenseClassDTO FindById(int licenseClassId) => _repository.GetLicenseClassInfoById(licenseClassId);

        public LicenseClassDTO FindByClassName(string className) => _repository.GetLicenseClassInfoByClassName(className);

        public DataTable GetAllLicenseClasses() => _repository.GetAllLicenseClasses();
    }

}