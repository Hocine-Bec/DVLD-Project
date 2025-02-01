using DVLD.DTOs;

namespace DVLD_Business
{
    public class DetainedMapper
    {
        private PersonService _personService;
        private UserService _userService;

        public DetainedMapper()
        {
            _personService = new PersonService();
            _userService = new UserService();
        }

        public DetainedDTO ToDTO(Detained detained)
        {
            return new DetainedDTO()
            {
                DetainId = detained.DetainId,
                LicenseId = detained.LicenseId,
                DetainDate = detained.DetainDate,
                FineFees = detained.FineFees,
                CreatedByUserId = detained.CreatedByUserId,
                IsReleased = detained.IsReleased,
                ReleaseDate = detained.ReleaseDate,
                ReleasedByUserId = detained.ReleasedByUserId,
                ReleaseAppId = detained.ReleaseAppId,
            };
        }

        public Detained FromDTO(DetainedDTO dto)
        {
            return new Detained()
            {
                DetainId = dto.DetainId,
                LicenseId = dto.LicenseId,
                DetainDate = dto.DetainDate,
                FineFees = dto.FineFees,
                CreatedByUserId = dto.CreatedByUserId,
                CreatedByUser = _userService.FindByUserID(dto.CreatedByUserId),
                IsReleased = dto.IsReleased,
                ReleaseDate = dto.ReleaseDate,
                ReleasedByUserId = dto.ReleasedByUserId,
                ReleaseAppId = dto.ReleaseAppId,
                ReleasedByUser = _userService.FindByPersonID(dto.ReleasedByUserId)
            };
        }

    }

}
