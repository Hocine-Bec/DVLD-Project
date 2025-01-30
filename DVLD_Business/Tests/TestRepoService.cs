using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class TestRepoService
    {
        private TestRepository _repository;

        public TestRepoService()
        {
            _repository = new TestRepository();
        }

        public int AddNew(TestsDTO dto) => _repository.AddNewTest(dto);

        public bool Update(TestsDTO dto) => _repository.UpdateTest(dto);

        public TestsDTO FindById(int testId) => _repository.GetTestInfoById(testId);

        public TestsDTO FindBy(int personId, int licenseClassId, int testTypeId)
            => _repository.GetLastTestByPersonAndTestTypeAndLicenseClass(personId, licenseClassId, testTypeId);

        public DataTable GetAllTests() => _repository.GetAllTests();

        public byte GetPassedTestCount(int localDrivingLicenseApplicationId)
            => _repository.GetPassedTestCount(localDrivingLicenseApplicationId);

        public bool IsThereAnActiveScheduledTest(int localLicenseAppId, int TestTypeID)
            => _repository.IsThereAnActiveScheduledTest(localLicenseAppId, TestTypeID);

        public byte TotalTrialsPerTest(int localLicenseAppId, int TestTypeID)
            => _repository.TotalTrialsPerTest(localLicenseAppId, TestTypeID);

        public byte AttendedTest(int localLicenseAppId, int TestTypeID)
            => _repository.TotalTrialsPerTest(localLicenseAppId, TestTypeID);

        public bool DoesPassTestType(int localLicenseAppId, int testTypeId)
            => _repository.DoesPassTestType(localLicenseAppId, testTypeId);

        public bool DoesAttendTestType(int localLicenseAppId, int testTypeId)
            => _repository.DoesAttendTestType(localLicenseAppId, testTypeId);


    }

}