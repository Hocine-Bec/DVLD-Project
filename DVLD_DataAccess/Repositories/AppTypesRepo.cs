using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Applications;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class AppTypesRepo : BaseRepoHelper, IAppTypeRepo
    {
        private readonly AppDbContext _context;

        public AppTypesRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<int> AddAppTypeAsync(AppType? appType)
        {
            if(appType is null)
                throw new ArgumentNullException(nameof(appType));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.ApplicationTypes.AddAsync(appType);
                await _context.SaveChangesAsync();
                return appType.Id;
            });
        }

        public async Task<List<AppType>?> GetAllAppTypesAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.ApplicationTypes.ToListAsync());
        }

        public async Task<AppType?> GetByIdAsync(int appTypeId)
        {
            if (appTypeId <= 0)
                throw new ArgumentException($"Application Type ID {appTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.ApplicationTypes.FindAsync(appTypeId));
        }

        public async Task<bool> UpdateAppTypeAsync(AppType? appType)
        {
            if (appType is null)
                throw new ArgumentNullException(nameof(appType));

            _context.ApplicationTypes.Update(appType);
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }
    }

}
