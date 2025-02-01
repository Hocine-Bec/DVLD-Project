using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace DVLD_Business
{
    public class TestAppointmentService
    {
        private readonly TestAppointmentRepoService _repoService;
        private readonly TestAppointmentMapper _mapper;

        public TestAppointmentService()
        {
            _repoService = new TestAppointmentRepoService();
            _mapper = new TestAppointmentMapper();
        }

        public bool AddNewTestAppointment(TestAppointment testAppointment)
        {
            if (TestAppointmentValidator.IsTestAppointmentObjectEmpty(testAppointment))
                return false;

            var dto = _mapper.ToDTO(testAppointment);

            dto.TestAppointmentId = _repoService.AddNew(dto);

            return (dto.TestAppointmentId != -1);
        }

        public bool UpdateTestAppointment(TestAppointment testAppointment)
        {
            if (TestAppointmentValidator.IsTestAppointmentObjectEmpty(testAppointment))
                return false;

            var dto = _mapper.ToDTO(testAppointment);

            return _repoService.Update(dto);
        }

        public TestAppointment Find(int testAppointmentId)
        {
            var dto = _repoService.Find(testAppointmentId);


            if (TestAppointmentValidator.IsTestAppointmentDTOEmpty(dto))
                return null;


            return _mapper.FromDTO(dto);
        }

        public TestAppointment GetLastTestAppointment(int localLicenseAppId, enTestType testTypeId)
        {
            var dto = (_repoService.GetLastTestAppointment(localLicenseAppId, (int)testTypeId));


            if (TestAppointmentValidator.IsTestAppointmentDTOEmpty(dto))
                return null;


            return _mapper.FromDTO(dto);
        }

        public DataTable GetAllTestAppointments() => _repoService.GetAllTestAppointments();

        public DataTable GetAppTestAppointmentsPerTestType(int localLicenseAppId, enTestType testTypeId)
            => _repoService.GetAppointmentsPerTestType(localLicenseAppId, (int)testTypeId);
    }

}
