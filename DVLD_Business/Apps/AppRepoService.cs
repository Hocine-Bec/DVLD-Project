using DVLD.DTOs;
using DVLD_DataAccess;


namespace DVLD_Business
{
    public class AppRepoService
    {
        private AppRepository _repository;

        public AppRepoService()
        {
            _repository = new AppRepository();
        }

        public int AddNewApp(AppDTO dto) => _repository.AddNewApplication(dto);

        public bool UpdateApp(AppDTO dto) => _repository.UpdateApplication(dto);

        public AppDTO FindBaseApp(int appId) => _repository.GetApplicationInfoById(appId);

        public bool Cancel(int appId) => _repository.UpdateStatus(appId, 2);

        public bool SetComplete(int appId) => _repository.UpdateStatus(appId, 3);

        public bool Delete(int appId) => _repository.DeleteApplication(appId);

        public bool IsAppExist(int appId) => _repository.IsApplicationExist(appId);

        public int GetActiveAppIdForLicenseClass(int personId, int appTypeId, int licenseClassId)
            => _repository.GetActiveAppIdForLicenseClass(personId, appTypeId, licenseClassId);

        public bool DoesPersonHaveActiveApplication(int personId, int appTypeId)
           => _repository.DoesPersonHaveActiveApplication(personId, appTypeId);

        public int GetActiveApplicationId(int personId, int appTypeId)
            => _repository.GetActiveApplicationId(personId, appTypeId);

    }
}
