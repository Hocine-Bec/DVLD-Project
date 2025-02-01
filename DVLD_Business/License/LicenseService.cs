using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD_Business;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_Business.LicenseService;

namespace DVLD_Business
{
    public enum enIssueReason
    {
        FirstTime = 1,
        Renew = 2,
        DamagedReplacement = 3,
        LostReplacement = 4
    };

    public class LicenseService
    {
        private LicenseRepoService _licenseRepoService;
        private LicenseMapper _licenseMapper;
        private AppService _appService;
      
        public LicenseService()
        {
            _licenseRepoService = new LicenseRepoService();
            _licenseMapper = new LicenseMapper();
            _appService = new AppService();
        }


        public License CreateNewLicense(License license, string issueReason, int userId, string notes = "")
        {
            var newLicense = new License()
            {
                DriverId = license.DriverId,
                LicenseClass = license.LicenseClass,
                IssueDate = DateTime.Now,
                ExpirationDate = license.ExpirationDate,
                Notes = license.Notes,
                PaidFees = 0,// no fees for the license because it's a replacement.
                IsActive = true,
                UserID = userId,
            };

            if (!string.IsNullOrEmpty(notes))
            {
                newLicense.Notes = notes;
            }


            return newLicense;
        }

        public bool AddNewLicense(License license)
        {
            if(LicenseValidator.IsLicenseObjectEmpty(license)) 
                return false;

            var dto = _licenseMapper.ToDTO(license);

            license.LicenseId = _licenseRepoService.AddNewLicense(dto);

            return (license.LicenseId != -1);
        }

        public bool UpdateLicense(License license)
        {
            if (LicenseValidator.IsLicenseObjectEmpty(license))
                return false;

            var dto = _licenseMapper.ToDTO(license);
            return _licenseRepoService.UpdateLicense(dto);
        }

        public License Find(int LicenseID)
        {
            var dto = _licenseRepoService.Find(LicenseID);

            if (LicenseValidator.IsLicenseDTOEmpty(dto))
                return null;

            return _licenseMapper.FromDTO(dto);
        }

        public DataTable GetAllLicenses() => _licenseRepoService.GetAllLicenses();
        
        public bool IsLicenseExistByPersonID(int personID, int licenseClassId)
            => (GetActiveLicenseIdByPersonId(personID, licenseClassId) != -1);

        public int GetActiveLicenseIdByPersonId(int personID, int licenseClassId)
            => _licenseRepoService.GetActiveLicenseIdByPersonId(personID, licenseClassId);

        public DataTable GetDriverLicenses(int driverId) => _licenseRepoService.GetDriverLicenses(driverId);

        public bool IsLicenseExpired(DateTime expireDate) => LicenseValidator.IsLicenseExpired(expireDate);

        public bool DeactivateCurrentLicense(int licenseId)
            => _licenseRepoService.DeactivateCurrentLicense(licenseId);

    }

    
}
