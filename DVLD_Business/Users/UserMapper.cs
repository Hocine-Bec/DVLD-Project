using DVLD.DTOs;

namespace DVLD_Business
{
    public class UserMapper
    {
        public static UsersDTO ToDTO(User user)
        {
            return new UsersDTO()
            {
                UserID = user.UserID,
                PersonID = user.PersonID,
                Username = user.UserName,
                Password = user.Password,
                IsActive = user.IsActive
            };
        }

        public static User FromDTO(UsersDTO userDTO, PersonService personService)
        {
            return new User()
            {
                UserID = userDTO.UserID,
                PersonID = userDTO.PersonID,
                UserName = userDTO.Username,
                Password = userDTO.Password,
                IsActive = userDTO.IsActive,
                Person = personService.Find(userDTO.PersonID)
            };
        }
    }

}
