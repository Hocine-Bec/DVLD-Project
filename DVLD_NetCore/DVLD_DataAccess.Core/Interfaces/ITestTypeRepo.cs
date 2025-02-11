using DVLD_DataAccess.Core.Entities;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ITestTypeRepo
    {
        public Task<TestType?> GetByIdAsync(int testTypeId);
        public Task<List<TestType>?> GetAllTestTypesAsync();
        public Task<int> AddTestTypeAsync(TestType testTypes);
        public Task<bool> UpdateTestTypeAsync(TestType testTypes);
    }

}
