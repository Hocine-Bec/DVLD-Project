using DVLD_DataAccess.Core.Entities;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ITestRepo
    {
        public Task<Test?> GetByIdAsync(int testId);
        public Task<Test?> GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(int personId, int licenseClassId, int testTypeId);
        public Task<List<Test>?> GetAllTestsAsync();
        public Task<int> AddTestAsync(Test? testsDTO);
        public Task<bool> UpdateTestAsync(Test? testsDTO);
        public Task<byte> GetPassedTestCountAsync(int localLicenseAppId);
        public Task<bool> DoesPassTestTypeAsync(int localLicenseAppId, int testTypeId);
        public Task<bool> DoesAttendTestTypeAsync(int localLicenseAppId, int testTypeId);
        public Task<byte> TotalTrialsPerTestAsync(int localLicenseAppId, int testTypeId);
        public Task<bool> IsThereAnActiveScheduledTestAsync(int localLicenseAppId, int testTypeId);
    }

}
