using DVLD.DTOs;


namespace DVLD_Business
{
    public class AppMapper
    {
        private static PersonService _personService = new PersonService();
        private static UserService _userService= new UserService();

        public AppDTO ToDTO(App app)
        {
            return new AppDTO()
            {
                AppId = app.AppId,
                PersonId = app.PersonId,
                AppDate = app.AppDate,
                AppTypeId = app.AppType.Id,
                StatusId = AppHelpers.SetStatusId(app.StatusText),
                LastStatusDate = app.LastStatusDate,
                PaidFees = app.PaidFees,
                UserID = app.CreatedByUser.UserID
            };
        }

        public App FromDTO(AppDTO appDTO)
        {
            return new App()
            {
                AppId = appDTO.AppId,
                PersonId = appDTO.PersonId,
                ApplicantFullName = _personService.Find(appDTO.PersonId).FullName,
                AppDate = appDTO.AppDate,
                AppType = AppType.Find(appDTO.AppTypeId),
                StatusText = AppHelpers.SetStatusText(appDTO.StatusId),
                LastStatusDate = appDTO.LastStatusDate,
                PaidFees = appDTO.PaidFees,
                CreatedByUser = _userService.FindByUserID(appDTO.UserID)
            };
            
        }
    }
}
