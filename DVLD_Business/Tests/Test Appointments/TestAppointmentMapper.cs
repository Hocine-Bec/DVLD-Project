using DVLD.DTOs;

namespace DVLD_Business
{
    public class TestAppointmentMapper
    {
        private readonly AppService _appService;

        public TestAppointmentMapper()
        {
            _appService = new AppService();
        }

        public TestsAppointmentDTO ToDTO(TestAppointment testAppointment)
        {
            return new TestsAppointmentDTO()
            {
               TestAppointmentId = testAppointment.TestAppointmentId,
               TestTypeId = (int)testAppointment.TestTypeId,
               LocalLicenseAppId = testAppointment.LocalLicenseAppId,
               AppointmentDate = testAppointment.AppointmentDate,
               PaidFees = testAppointment.PaidFees,
               UserId = testAppointment.UserId,
               IsLocked = testAppointment.IsLocked,
               RetakeTestAppId = testAppointment.RetakeTestAppId
            };
        }

        public TestAppointment FromDTO(TestsAppointmentDTO dto)
        {
            return new TestAppointment()
            {
                TestAppointmentId = dto.TestAppointmentId,
                TestTypeId = (enTestType)dto.TestTypeId,
                LocalLicenseAppId = dto.LocalLicenseAppId,
                AppointmentDate = dto.AppointmentDate,
                PaidFees = dto.PaidFees,
                UserId = dto.UserId,
                IsLocked = dto.IsLocked,
                RetakeTestAppId = dto.RetakeTestAppId,
                RetakeTestApp = _appService.FindBaseApp(dto.RetakeTestAppId),
                TestId= dto.TestId
            };
        }
    }
}
