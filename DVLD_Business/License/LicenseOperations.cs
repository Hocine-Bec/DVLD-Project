using System;

namespace DVLD_Business
{
    public class LicenseOperations
    {
        private readonly LicenseService _licenseService;
        private readonly AppService _appService;
        private readonly clsDetainedLicense _detainedLicense;

        public LicenseOperations()
        {
            _licenseService = new LicenseService();
            _appService = new AppService();
            _detainedLicense = new clsDetainedLicense();
        }

        public bool Release(License license, int userId, ref int appId)
        {
            short appTypeId = (short)enAppType.ReleaseDetainedDrivingLicense;

            //Create New
            var app = _appService.CreateNewLicenseApp(license.Driver.PersonID, userId, appTypeId);

            if (!_appService.AddNewApp(app))
            {
                appId = -1;
                return false;
            }

            appId = app.AppId;


            return _detainedLicense.ReleaseDetainedLicense(userId, app.AppId);
        }

        public License Renew(License license, string notes, int userId)
        {
            //we need to deactivate the old License.
            if (_licenseService.DeactivateCurrentLicense(license.LicenseId))
                return null;


            short appTypeId = (short)enAppType.RenewDrivingLicense;

            //Create New App
            var app = _appService.CreateNewLicenseApp(license.Driver.PersonID, userId, appTypeId);


            if (!_appService.AddNewApp(app))
                return null;

            string issueReason = enIssueReason.Renew.ToString();
            var newLicense = _licenseService.CreateNewLicense(license, issueReason, userId);

            newLicense.PaidFees = license.LicenseClass.ClassFees;

            int defaultValidityLength = license.LicenseClass.DefaultValidityLength;
            newLicense.ExpirationDate = DateTime.Now.AddYears(defaultValidityLength);


            if (!_licenseService.AddNewLicense(newLicense))
                return null;


            return newLicense;
        }

        public License Replace(enIssueReason issueReason, int userId, License license)
        {
            //we need to deactivate the old License.
            if (_licenseService.DeactivateCurrentLicense(license.LicenseId))
                return null;


            short appTypeId = GetLostOrDamagedLicenseId(issueReason.ToString());
            //Create New App
            var app = _appService.CreateNewLicenseApp(license.Driver.PersonID, userId, appTypeId);

            if (!_appService.AddNewApp(app))
                return null;

            var newLicense = _licenseService.CreateNewLicense(license, issueReason.ToString(), userId);
            newLicense.AppId = app.AppId;

            if (!_licenseService.AddNewLicense(newLicense))
            {
                return null;
            }

            return newLicense;
        }

        private short GetLostOrDamagedLicenseId(string issueReason)
        {
            var appType = (issueReason == enIssueReason.DamagedReplacement.ToString())
                                ? enAppType.ReplaceDamagedDrivingLicense
                                : enAppType.ReplaceLostDrivingLicense;

            return (short)appType;
        }
    }

    
}
