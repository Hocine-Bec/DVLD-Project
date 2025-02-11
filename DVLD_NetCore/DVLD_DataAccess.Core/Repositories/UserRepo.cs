using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Identity;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class UserRepo : BaseRepoHelper, IUserRepo
    {
        private readonly AppDbContext _context;

        public UserRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<User?> GetByUserIdAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException($"User ID {userId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.FindAsync(userId));
        }

        public async Task<User?> GetByPersonIdAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.FirstOrDefaultAsync(x 
                => x.PersonId == personId));
        }

        public async Task<User?> GetByUsernameAndPasswordAsync(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Username and password cannot be null or empty.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.FirstOrDefaultAsync(x => 
            x.Username == userName && x.Password == password));
        }

        public async Task<int> AddNewUserAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException("User cannot be null.");


            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user.Id;
            });
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException("User cannot be null.");

            ExecuteDbOperation(() => _context.Users.Update(user));
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }

        public async Task<List<User>?> GetAllUsersAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.Users.ToListAsync());
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException($"User ID {userId} must be greater than 0.");

            var user = await ExecuteDbOperationAsync(async () => await _context.Users.FindAsync(userId));

            if (user is null)
                throw new ArgumentNullException("User cannot be null.");

            _context.Users.Remove(user);
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> DoesUserExistAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException($"User ID {userId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.AnyAsync(x => x.Id == userId));
        }

        public async Task<bool> DoesUserExistAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("Username cannot be null or empty.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.AnyAsync(x => x.Username == userName));
        }

        public async Task<bool> DoesPersonHaveUserAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Users.AnyAsync(x => x.PersonId == personId));
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            if (userId <= 0)
                throw new ArgumentException($"User ID {userId} must be greater than 0.");

            var user = await ExecuteDbOperationAsync(async () => await _context.Users.FindAsync(userId));

            if (user is null)
                throw new ArgumentNullException("User cannot be null.");

            user.Password = newPassword;

            _context.Users.Update(user);
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }
    }
}
