using DVLD_DataAccess.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IUserRepo
    {
        public Task<User?> GetByUserIdAsync(int userId);
        public Task<User?> GetByPersonIdAsync(int personId);
        public Task<User?> GetByUsernameAndPasswordAsync(string userName, string password);
        public Task<int> AddNewUserAsync(User user);
        public Task<bool> UpdateUserAsync(User user);
        public Task<List<User>?> GetAllUsersAsync();
        public Task<bool> DeleteUserAsync(int userId);
        public Task<bool> DoesUserExistAsync(int userId);
        public Task<bool> DoesUserExistAsync(string userName);
        public Task<bool> DoesPersonHaveUserAsync(int personId);
        public Task<bool> ChangePasswordAsync(int userId, string newPassword);
    }
}
