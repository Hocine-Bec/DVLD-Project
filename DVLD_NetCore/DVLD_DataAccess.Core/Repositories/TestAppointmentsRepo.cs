using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class TestAppointmentRepo : BaseRepoHelper, ITestAppointmentRepo
    {
        private readonly AppDbContext _context;

        public TestAppointmentRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<TestAppointment?> GetByIdAsync(int testAppointmentId)
        {
            if (testAppointmentId <= 0)
                throw new ArgumentException($"Test Appointment ID {testAppointmentId} must be greater than 0.");

            const string query =@"
             SELECT TestAppointments.*, TestID FROM TestAppointments 
             INNER JOIN Tests ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID";

            return await ExecuteDbOperationAsync(async () =>
                await _context.TestAppointments.FromSqlRaw(query)
                .FirstOrDefaultAsync(x => x.Id == testAppointmentId));
        }

        public async Task<TestAppointment?> GetLastTestAppointmentAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.TestAppointments.FirstOrDefaultAsync
            (
                x => x.LocalLicenseAppId == localLicenseAppId
                && x.TestTypeId == testTypeId)
            );
        }

        public async Task<List<TestAppointment>?> GetAllTestAppointmentsAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.TestAppointments
                    .OrderByDescending(x => x.AppointmentDate)
                    .ToListAsync());
        }

        public async Task<List<TestAppointment>?> GetAppTestAppointmentsPerTestTypeAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.TestAppointments
                .Where(x => x.LocalLicenseAppId == localLicenseAppId && x.TestTypeId == testTypeId)
                .ToListAsync());
        }

        public async Task<int> AddTestAppointmentAsync(TestAppointment testAppointment)
        {
            if (testAppointment == null)
                throw new ArgumentNullException(nameof(testAppointment));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.TestAppointments.AddAsync(testAppointment);
                await _context.SaveChangesAsync();
                return testAppointment.Id;
            });
        }

        public async Task<bool> UpdateTestAppointmentAsync(TestAppointment testAppointment)
        {
            if (testAppointment == null)
                throw new ArgumentNullException(nameof(testAppointment));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.TestAppointments.Update(testAppointment);
                return await _context.SaveChangesAsync() > 0;
            });
        }

       
    }
}