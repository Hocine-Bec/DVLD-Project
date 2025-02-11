using DVLD_DataAccess.Core.Entities;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ITestAppointmentRepo
    {
        public Task<TestAppointment?> GetByIdAsync(int testAppointmentId);
        public Task<TestAppointment?> GetLastTestAppointmentAsync(int localLicenseAppId, int testTypeId);
        public Task<List<TestAppointment>?> GetAllTestAppointmentsAsync();
        public Task<List<TestAppointment>?> GetAppTestAppointmentsPerTestTypeAsync(int localLicenseAppId, int testTypeId);
        public Task<int> AddTestAppointmentAsync(TestAppointment testsAppointments);
        public Task<bool> UpdateTestAppointmentAsync(TestAppointment testsAppointments);
    }

}
