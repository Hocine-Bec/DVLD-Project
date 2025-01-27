using DVLD.DTOs;

namespace DVLD_Business
{
    public class UserValidator
    {
        public bool IsUserDTOEmpty(UsersDTO usersDTO) => (usersDTO == null) ? true : false;

        public bool IsUserObjectEmpty(User user) => (user == null) ? true : false;

    }

}
