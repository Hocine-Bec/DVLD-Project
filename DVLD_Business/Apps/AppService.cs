using System;
using System.CodeDom;
using System.Data;
using System.Security.AccessControl;
using DVLD.DTOs;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_Business.AppService;


namespace DVLD_Business
{
    public enum enAppType
    {
        NewDrivingLicense = 1,
        RenewDrivingLicense = 2,
        ReplaceLostDrivingLicense = 3,
        ReplaceDamagedDrivingLicense = 4,
        ReleaseDetainedDrivingLicense = 5,
        NewInternationalLicense = 6,
        RetakeTest = 7
    };

    public enum enAppStatus
    {
        New = 1,
        Cancelled = 2,
        Completed = 3
    };

    public class AppService
    {
        //This is for testing purpose, it will be updated later
        private readonly AppRepoService _appRepoService;
        private readonly AppMapper _appMapper;
        
        public AppService()
        {
            PersonService _personService = new PersonService();
            UserService _userService = new UserService();
            AppType _appType = new AppType();
            AppRepoService _appRepoService = new AppRepoService();
            AppMapper _appMapper = new AppMapper();
        }

        public bool AddNewApp(App app)
        {
            if (AppValidator.IsAppObjectEmpty(app))
                return false;

            var dto = _appMapper.ToDTO(app);

            app.AppId = _appRepoService.AddNewApp(dto);

            return (app.AppId != -1);
        }
        
        public bool UpdateApp(App app)
        {
            if (AppValidator.IsAppObjectEmpty(app))
                return false;


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

        public App CreateNewLicenseApp(int personId, int userId, short appTypeId)
        {
            var app = new App()
            {
                PersonId = personId,
                AppDate = DateTime.Now,
                AppTypeId = appTypeId,
                StatusText = enAppStatus.Completed.ToString(),
                LastStatusDate = DateTime.Now,
                PaidFees = AppType.Find(appTypeId).Fees,
                UserId = userId,
            };

            return (this.AddNewApp(app)) ? app : null;
        }

    }
}


