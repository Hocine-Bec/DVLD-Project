using DVLD.DTOs;
using DVLD_Business;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;


namespace DVLD_Business
{
    public class LicenseIssuer
    {
        private readonly LocalLicenseApp _licenseApp;
        private readonly DriverService _driverService;
        private readonly AppService _appService;
        private readonly LicenseService _licenseService;

        public LicenseIssuer(LocalLicenseApp licenseApp)
        {
            _licenseApp = licenseApp;
            _appService = new AppService();
            _driverService = new DriverService();
            _licenseService = new LicenseService();
        }

        public int IssueLicenseForTheFirstTime(string notes, int userId)
        {
            //Get driver Id or create a new one
            int driverId = GetOrCreateDriver(_licenseApp.App, userId);
            if (driverId == -1)
                return -1;

            //Create New license
            var license = CreateNewLocalLicense(notes, userId, driverId);
            if (license.LicenseId == -1)
                return -1;

            _appService.SetComplete(license.AppId);
            return license.LicenseId;
        }

        public int GetOrCreateDriver(App app, int userId)
        {
            int driverId = -1;
            Driver driver = _driverService.FindByPersonId(app.ApplicantPersonId);

            //we check if the driver already there for this person.
            if (driver == null)
            {
                driverId = _driverService.AddNewDriver(app.ApplicantPersonId, userId);

                if (driverId == -1)
                {
                    return -1;
                }

            }
            else
            {
                driverId = driver.DriverId;
            }

            return driverId;
        }

        public License CreateNewLocalLicense(string notes, int userId, int driverId)
        {
            var license = new License()
            {
                AppId = _licenseApp.App.AppId,
                DriverId = driverId,
                LicenseClass = _licenseApp.LicenseClass,
                IssueDate = DateTime.Now,
                ExpirationDate = DateTime.Now.AddYears(_licenseApp.LicenseClass.DefaultValidityLength),
                Notes = notes,
                PaidFees = _licenseApp.LicenseClass.ClassFees,
                IsActive = true,
                IssueReasonText = enIssueReason.FirstTime.ToString(),
                UserID = userId
            };

            _licenseService.AddNewLicense(license);

            return license;
        }

        public bool IsLicenseIssued() => (GetActiveLicenseID() != -1);

        public int GetActiveLicenseID()
        {
            //this will get the license ID that belongs to this application
            return _licenseService.GetActiveLicenseIdByPersonId(_licenseApp.App.AppId,
                _licenseApp.LicenseClass.LicenseClassID);
        }

    }

}








