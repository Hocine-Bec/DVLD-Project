using DVLD.DTOs;

namespace DVLD_Business
{
    public class LicenseTypeMapper
    {
        public LicenseClassDTO ToDTO(LicenseType licenseClass)
        {
            return new LicenseClassDTO()
            {
                LicenseClassID = licenseClass.LicenseClassID,
                ClassName = licenseClass.ClassName,
                ClassDescription = licenseClass.ClassDescription,
                MinimumAllowedAge = licenseClass.MinimumAllowedAge,
                DefaultValidityLength = licenseClass.DefaultValidityLength,
                ClassFees = licenseClass.ClassFees
            };
        }

        public LicenseType FromDTO(LicenseClassDTO dto)
        {
            return new LicenseType()
            {
                LicenseClassID = dto.LicenseClassID,
                ClassName = dto.ClassName,
                ClassDescription = dto.ClassDescription,
                MinimumAllowedAge = dto.MinimumAllowedAge,
                DefaultValidityLength = dto.DefaultValidityLength,
                ClassFees = dto.ClassFees
            };
        }
    }

}