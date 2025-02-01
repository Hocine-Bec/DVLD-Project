using DVLD.DTOs;

namespace DVLD_Business
{
    public class InternationalMapper
    {
        private readonly PersonService _personService;
        private readonly UserService _userService;
        private readonly AppService _appService;
        private readonly DriverService _driverService;

        public InternationalMapper()
        {
            _personService = new PersonService();
            _userService = new UserService();
            _appService = new AppService();
            _driverService = new DriverService();
        }

        public InternationalDTO ToDTO(International international)
        {
            return new InternationalDTO()
            {
                AppId = international.App.AppId,
                InternationalId = international.InternationalId,
                DriverId = international.DriverID,
                IssuedUsingLicenseId = international.IssuedUsingLicenseId,
                IssueDate = international.IssueDate,
                ExpireDate = international.ExpireDate,
                IsActive = international.IsActive,
                UserId = international.UserId
            };
        }

        public International FromDTO(InternationalDTO dto)
        {
            return new International()
            {
                InternationalId = dto.InternationalId,
                App = _appService.FindBaseApp(dto.AppId),
                Driver = _driverService.FindByDriverId(dto.DriverId),
                IssuedUsingLicenseId = dto.IssuedUsingLicenseId,
                IssueDate = dto.IssueDate,
                ExpireDate = dto.ExpireDate,
                IsActive = dto.IsActive,
            };
            
        }
    }

}
