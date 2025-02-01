using DVLD.DTOs;

namespace DVLD_Business
{
    public class LicenseTypeValidator
    {
        public static bool IsLicenseClassDTOEmpty(LicenseClassDTO dto) => (dto == null) ? true : false;

        public static bool IsLicenseClassObjectEmpty(LicenseType licenseClass) => (licenseClass == null) ? true : false;
    }

}