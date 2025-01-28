﻿using System.CodeDom;
using System.Data;
using System.Security.AccessControl;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_Business.AppService;


namespace DVLD_Business
{
    public class AppService
    {
        //This is for testing purpose, it will be updated later
        private readonly PersonService _personService;
        private readonly UserService _userService;
        private readonly AppType _appType;
        private readonly AppRepoService _appRepoService;
        private readonly AppMapper _appMapper;
        
        public enum enApplicationType 
        {
            NewDrivingLicense = 1, 
            RenewDrivingLicense = 2, 
            ReplaceLostDrivingLicense=3,
            ReplaceDamagedDrivingLicense=4, 
            ReleaseDetainedDrivingLicense=5, 
            NewInternationalLicense=6,
            RetakeTest=7
        };

        public enum enApplicationStatus 
        { 
            New = 1, 
            Cancelled = 2,
            Completed = 3
        };

        public AppService()
        {
            PersonService _personService = new PersonService();
            UserService _userService = new UserService();
            AppType _appType = new AppType();
            AppRepoService _appRepoService = new AppRepoService();
            AppMapper _appMapper = new AppMapper();
        }

        public static string SetStatusText(int statusId)
        {
            switch ((enApplicationStatus)statusId)
            {
                case enApplicationStatus.New:
                    return "New";
                case enApplicationStatus.Cancelled:
                    return "Cancelled";
                case enApplicationStatus.Completed:
                    return "Completed";
                default:
                    return "Unknown";
            }
        }

        public static short SetStatusId(string status)
        {
            switch (status)
            {
                case "New":
                    return 1;
                case "Cancelled":
                    return 2;
                case "Completed":
                    return 3;
                default:
                    return -1;
            }
        }

        public bool AddNewApp(App app)
        {
            var dto = _appMapper.ToDTO(app);

            app.AppId = _appRepoService.AddNewApp(dto);

            return (app.AppId != -1);
        }
        
        public bool UpdateApp(App app)
        {
            var dto = _appMapper.ToDTO(app);

            return _appRepoService.UpdateApp(dto);
        }

        public App FindBaseApp(int appId)
        {
            var dto = _appRepoService.FindBaseApp(appId);

            if (AppValidator.IsAppDTOEmpty(dto))
            {
                return null;
            }

            return _appMapper.FromDTO(dto);
        }
        
        public bool Cancel(int appId) => _appRepoService.Cancel(appId);

        public bool SetComplete(int appId) => _appRepoService.SetComplete(appId);

        public  bool Delete(int appId) => _appRepoService.Delete(appId);

        public bool IsAppExist(int appId) => _appRepoService.IsAppExist(appId);

        public int GetActiveAppIdForLicenseClass(int personId, int appTypeId, int licenseClassId)
            => _appRepoService.GetActiveAppIdForLicenseClass(personId, appTypeId, licenseClassId);

        public bool DoesPersonHaveActiveApplication(int personId, int appTypeId)
            => _appRepoService.DoesPersonHaveActiveApplication(personId, appTypeId);

        public int GetActiveApplicationId(int personId, int appTypeId)
            => _appRepoService.GetActiveApplicationId(personId, appTypeId);

    }
}
