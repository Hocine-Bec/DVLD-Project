using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class UserRepoService
    {
        private UsersRepository _usersRepository;

        public UserRepoService()
        {
            _usersRepository = new UsersRepository();
        }

        public int AddNewUser(UsersDTO usersDTO) => _usersRepository.AddNewUser(usersDTO);

        public bool UpdateUser(UsersDTO usersDTO) => _usersRepository.UpdateUser(usersDTO);

        public UsersDTO FindByUserID(int UserID) => _usersRepository.GetUserInfoByUserId(UserID);

        public UsersDTO FindByPersonID(int PersonID) => _usersRepository.GetUserInfoByPersonId(PersonID);

        public UsersDTO FindByUsernameAndPassword(string username, string password)
              => _usersRepository.GetUserInfoByUsernameAndPassword(username, password);

        public DataTable GetAllUsers() => _usersRepository.GetAllUsers();

        public bool DeleteUser(int UserID) => _usersRepository.DeleteUser(UserID);

        public bool IsUserExist(int UserID) => _usersRepository.IsUserExist(UserID);

        public bool IsUserExist(string UserName) => _usersRepository.IsUserExist(UserName);

        public bool IsUserExistByPersonId(int PersonID) => _usersRepository.IsUserExistByPersonId(PersonID);

    }

}
