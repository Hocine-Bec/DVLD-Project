using DVLD.DTOs;

namespace DVLD_Business
{
    public class DriverMapper
    {
        private PersonService _personService;

        public DriverMapper()
        {
            _personService = new PersonService();
        }

        public DriverDTO ToDTO(Driver driver)
        {
            return new DriverDTO()
            {
                DriverID = driver.DriverId,
                PersonID = driver.PersonId,
                CreatedByUserID = driver.UserId,
                CreatedDate = driver.CreatedDate
            };
        }

        public Driver FromDTO(DriverDTO dto)
        {
            var person = _personService.Find(dto.PersonID);

            return new Driver(dto, person);
        }
    }
}
