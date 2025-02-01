using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_Business;
using DVLD_DataAccess;
using static DVLD_Business.TestType;

namespace DVLD_Business
{
    public class TestService
    {
        private readonly TestRepoService _repoService;
        private readonly TestMapper _mapper;

        public TestService()
        {
            _repoService = new TestRepoService();
            _mapper = new TestMapper();
        }

        public int CreateTest(int testAppointmentId, bool testResult, string notes, int createdByUserId)
        {
            var test = new Test
            {
                TestAppointmentId = testAppointmentId,
                TestResult = testResult,
                Notes = notes,
                UserId = createdByUserId
            };

            if (!AddNewTest(test))
            {
                return -1;
            }

            return test.TestId;
        }

        public bool AddNewTest(Test test)
        {
            if (TestValidator.IsTestObjectEmpty(test))
                return false;

            var dto = _mapper.ToDTO(test);

            dto.TestId = _repoService.AddNew(dto);

            return (dto.TestId != -1);
        }

        public bool UpdateTest(Test test)
        {
            if (TestValidator.IsTestObjectEmpty(test))
                return false;

            var dto = _mapper.ToDTO(test);

            return _repoService.Update(dto);
        }

        public Test Find(int testId)
        {
            var dto = _repoService.FindById(testId);

            if (TestValidator.IsTestDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public Test FindLastTestPerPersonAndLicenseClass(int personId, int licenseClassId, enTestType testType)
        {
            var dto = _repoService.FindBy(personId, licenseClassId, (int)testType);

            if (TestValidator.IsTestDTOEmpty(dto))
                return null;

            return _mapper.FromDTO(dto);
        }

        public DataTable GetAllTests() => _repoService.GetAllTests();

       
    }

}