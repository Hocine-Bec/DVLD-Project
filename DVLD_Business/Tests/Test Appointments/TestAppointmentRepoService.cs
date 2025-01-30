using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class TestAppointmentRepoService
    {
        private readonly TestAppointmentRepository _repository;

        public TestAppointmentRepoService()
        {
            _repository = new TestAppointmentRepository();
        }

        public int AddNew(TestsAppointmentDTO dto) => _repository.AddNewTestAppointment(dto);
 
        public bool Update(TestsAppointmentDTO dto) => _repository.UpdateTestAppointment(dto);

        public TestsAppointmentDTO Find(int testAppointmentId)
            => _repository.GetTestAppointmentInfoById(testAppointmentId);

        public TestsAppointmentDTO GetLastTestAppointment(int localLicenseAppId, int testTypeId)
            => _repository.GetLastTestAppointment(localLicenseAppId, testTypeId);

        public DataTable GetAllTestAppointments() => _repository.GetAllTestAppointments();


        public DataTable GetAppointmentsPerTestType(int localLicenseAppId, int testTypeId)
            => _repository.GetApplicationTestAppointmentsPerTestType(localLicenseAppId, testTypeId);
    }
}
