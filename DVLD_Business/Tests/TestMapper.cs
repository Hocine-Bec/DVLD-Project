using DVLD.DTOs;

namespace DVLD_Business
{
    public class TestMapper
    {
        private readonly TestAppointmentService _testAppointmentService;

        public TestMapper()
        {
            _testAppointmentService = new TestAppointmentService();
        }

        public TestsDTO ToDTO(Test test)
        {
            return new TestsDTO()
            {
                TestId = test.TestId,
                TestAppointmentId = test.TestAppointmentId,
                TestResult = test.TestResult,
                Notes = test.Notes,
                UserId = test.UserId
            };
        }

        public Test FromDTO(TestsDTO dto)
        {
            return new Test()
            {
                TestId = dto.TestId,
                TestAppointmentId = dto.TestAppointmentId,
                TestAppointment = _testAppointmentService.Find(dto.TestAppointmentId),
                TestResult = dto.TestResult,
                Notes = dto.Notes,
                UserId = dto.UserId
            };
        }
    }

}