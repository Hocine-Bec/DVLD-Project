using System;
using DVLD.DTOs;

namespace DVLD_Business
{
    public class LicenseValidator
    {
        public static bool IsLicenseDTOEmpty(LicenseDTO appDTO) => (appDTO == null) ? true : false;

        public static bool IsLicenseObjectEmpty(License app) => (app == null) ? true : false;

        public static bool IsLicenseExpired(DateTime expireDate) => (expireDate < DateTime.Now);

    }

    
}
