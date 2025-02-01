using System;
using System.Data;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Xml.Linq;
using DVLD_Business;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_Business.TestType;


namespace DVLD_Business
{
    public class LocalLicenseAppService
    {
        //This is for testing purpose, it will be updated later
        private readonly PersonService _personService;
        private readonly AppService _appService;
        private readonly LicenseTypeService _licenseClass;
        private readonly LicenseService _licenseService;
        private readonly DriverService _driverService;
        private readonly TestService _testService;


        public enMode Mode = enMode.AddNew;

        public LocalLicenseApp localLicenseApp;

        public LocalLicenseAppService()
        {
            localLicenseApp = new LocalLicenseApp();

            _personService = new PersonService();
            _appService = new AppService();
            _licenseClass = new LicenseTypeService();
            _licenseService = new LicenseService();
            _driverService = new DriverService();
            _testService = new TestService();


            Mode = enMode.AddNew;
        }

        private LocalLicenseAppService(int localLicenseAppId, int appId, int licenseClassId)
        {
            localLicenseApp = new LocalLicenseApp()
            {
                LocalLicenseAppId = localLicenseAppId,
                App = _appService.FindBaseApp(appId),
                LicenseClass = _licenseClass.Find(licenseClassId),
                PersonFullName = _personService.Find(localLicenseApp.App.ApplicantPersonId).FullName,
            };

            Mode = enMode.Update;
        }

        private bool _AddNewLocalLicenseApp()
        {
            //call DataAccess Layer 
            localLicenseApp.LocalLicenseAppId = LocalLicenseAppRepository.AddNewLocalDrivingLicenseApplication
                (localLicenseApp.App.AppId, localLicenseApp.LicenseClass.LicenseClassID);

            return (localLicenseApp.LocalLicenseAppId != -1);
        }
        
        private bool _UpdateLocalLicenseApp()
        {
            //call DataAccess Layer 
            return LocalLicenseAppRepository.UpdateLocalDrivingLicenseApplication
                (localLicenseApp.LocalLicenseAppId, localLicenseApp.App.AppId, localLicenseApp.LicenseClass.LicenseClassID);
        }

        public static LocalLicenseAppService FindByLocalDrivingAppLicenseID(int localLicenseAppId)
        {
            int appId = -1, licenseClassId = -1;

            bool IsFound = LocalLicenseAppRepository.GetLocalDrivingLicenseApplicationInfoByID
                (localLicenseAppId, ref appId, ref licenseClassId);

            if (IsFound)
                return new LocalLicenseAppService(localLicenseAppId, appId, licenseClassId);
            else
                return null;
        }
 
        public static LocalLicenseAppService FindByApplicationID(int appId)
        {
            int localLicenseAppId = -1, licenseClassId = -1;

            if (LocalLicenseAppRepository.GetLocalDrivingLicenseApplicationInfoByApplicationId
                (appId, ref localLicenseAppId, ref licenseClassId))
            {

                return new LocalLicenseAppService(licenseClassId, appId, localLicenseAppId);
            }
            else
            {
                return null;
            }
        }
       
        public bool Save()
        {
            if (!_appService.AddNewApp(localLicenseApp.App))
                return false;

            //After we save the main application now we save the sub application.
            switch (Mode)
            {
                case enMode.AddNew:
                    if (this._AddNewLocalLicenseApp())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return this._UpdateLocalLicenseApp();

            }

            return false;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
            => LocalLicenseAppRepository.GetAllLocalDrivingLicenseApplications();
               
        public bool Delete()
        {
            bool IsLocalDrivingAppDeleted = false;
            bool IsBaseAppDeleted = false;

            //First we delete the Local Driving License Application
            IsLocalDrivingAppDeleted = 
                LocalLicenseAppRepository.DeleteLocalDrivingLicenseApplication(localLicenseApp.LocalLicenseAppId);
           
            if (!IsLocalDrivingAppDeleted)
                return false;

            //Then we delete the base Application
            IsBaseAppDeleted = _appService.Delete(localLicenseApp.App.AppId);

            return IsBaseAppDeleted;
        }

    }

}


