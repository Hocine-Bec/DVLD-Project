using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace DVLD_Business
{
    public class LicenseTypeService
    {
        private readonly LicenseTypeRepoService _repoService;
        private readonly LicenseTypeMapper _mapper;

        public LicenseTypeService()
        {
            _repoService = new LicenseTypeRepoService();
            _mapper = new LicenseTypeMapper();
        }

        public bool AddNewLicenseClass(LicenseType licenseClass)
        {
            if (LicenseTypeValidator.IsLicenseClassObjectEmpty(licenseClass))
                return false;

            var dto = _mapper.ToDTO(licenseClass);
            dto.LicenseClassID = _repoService.AddNew(dto);

            return (dto.LicenseClassID != -1);
        }

        public bool UpdateLicenseClass(LicenseType licenseClass)
        {
            if (LicenseTypeValidator.IsLicenseClassObjectEmpty(licenseClass))
                return false;

            var dto = _mapper.ToDTO(licenseClass);
            return _repoService.Update(dto);
        }

        public LicenseType Find(int licenseClassId)
        {
            var dto = _repoService.FindById(licenseClassId);

            if (LicenseTypeValidator.IsLicenseClassDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public LicenseType Find(string className)
        {
            var dto = _repoService.FindByClassName(className);

            if (LicenseTypeValidator.IsLicenseClassDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public DataTable GetAllLicenseClasses() => _repoService.GetAllLicenseClasses();
    }

}