using System;
using System.Data;
using System.Runtime.InteropServices;

namespace DVLD_Business
{
    public class UserService
    {
        private PersonService _personService;
        private UserRepoService _repositoryService;
        private UserMapper _userMapper;
        private UserValidator _userValidator;

        public UserService()
        {
            _personService = new PersonService();
            _repositoryService = new UserRepoService();
            _userMapper = new UserMapper();
            _userValidator = new UserValidator();
        }

        public bool AddNewUser(User user)
        {
            if (_userValidator.IsUserObjectEmpty(user))
            {
                return false;
            }

            var usersDTO = UserMapper.ToDTO(user);

            user.UserID = _repositoryService.AddNewUser(usersDTO);

            return (user.UserID != -1);
        }

        public bool UpdateUser(User user)
        {
            if (_userValidator.IsUserObjectEmpty(user))
            {
                return false;
            }

            var usersDTO = UserMapper.ToDTO(user);

            return _repositoryService.UpdateUser(usersDTO);
        }

        public User FindByUserID(int UserID)
        {
            var usersDTO = _repositoryService.FindByUserID(UserID);

            if (_userValidator.IsUserDTOEmpty(usersDTO))
                return null;

            return UserMapper.FromDTO(usersDTO, _personService);
        }

        public User FindByPersonID(int PersonID)
        {
            var usersDTO = _repositoryService.FindByPersonID(PersonID);

            if (_userValidator.IsUserDTOEmpty(usersDTO))
                return null;

            return UserMapper.FromDTO(usersDTO, _personService);
        }

        public User FindByUsernameAndPassword(string username, string password)
        {
            var usersDTO = _repositoryService.FindByUsernameAndPassword(username, password);

            if (_userValidator.IsUserDTOEmpty(usersDTO))
                return null;

            return UserMapper.FromDTO(usersDTO, _personService);
        }

        public DataTable GetAllUsers() => _repositoryService.GetAllUsers();

        public bool DeleteUser(int UserID) => _repositoryService.DeleteUser(UserID);

        public bool IsUserExist(int UserID) => _repositoryService.IsUserExist(UserID);

        public bool IsUserExist(string UserName) => _repositoryService.IsUserExist(UserName);

        public bool IsUserExistByPersonId(int PersonID) => _repositoryService.IsUserExistByPersonId(PersonID);
    }

}
