using DVLD_DataAccess.Core.Entities.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IAppTypeRepo
    {
        public Task<AppType?> GetByIdAsync(int appTypeId);
        public Task<List<AppType>?> GetAllAppTypesAsync();
        public Task<int> AddAppTypeAsync(AppType? appType);
        public Task<bool> UpdateAppTypeAsync(AppType? appType);
    }

}
