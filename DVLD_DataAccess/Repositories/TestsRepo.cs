using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class TestsRepo : BaseRepoHelper, ITestRepo
    {
        private readonly AppDbContext _context;

        public TestsRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<Test?> GetByIdAsync(int testId)
        {
            if (testId <= 0)
                throw new ArgumentException($"Test ID {testId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Tests.FindAsync(testId));
        }

        public async Task<Test?> GetLastTestByPersonAndTestTypeAndLicenseTypeAsync(int personId, int licenseTypeId, int testTypeId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            if (licenseTypeId <= 0)
                throw new ArgumentException($"License Class ID {licenseTypeId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            const string query = @"
        SELECT TOP 1 
            Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, 
            Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
        FROM LocalDrivingLicenseApplications 
        INNER JOIN Tests ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
        INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
        WHERE Applications.ApplicantPersonID = @PersonID 
            AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
            AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY Tests.TestAppointmentID DESC";

            return await ExecuteDbOperationAsync(async () =>
                await _context.Tests.FromSqlRaw(query,
                new SqlParameter("@PersonID", personId),
                new SqlParameter("@LicenseClassID", licenseTypeId),
                new SqlParameter("@TestTypeID", testTypeId))
                .OrderByDescending(x => x.TestAppointmentId)
                .FirstOrDefaultAsync());
        }

        public async Task<List<Test>?> GetAllTestsAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.Tests
                    .OrderBy(t => t.Id)
                    .ToListAsync());
        }

        public async Task<int> AddTestAsync(Test? test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.Tests.AddAsync(test);
                await LockTestAppointmentAsync(test);
                await _context.SaveChangesAsync();
                return test.Id;
            });
        }

        public async Task<bool> UpdateTestAsync(Test? test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.Tests.Update(test);
                return await _context.SaveChangesAsync() > 0;
            });
        }

        public async Task<byte> GetPassedTestCountAsync(int localLicenseAppId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            const string query = @"
        SELECT PassedTestCount = COUNT(TestTypeID)
        FROM Tests 
        INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
        WHERE LocalDrivingLicenseApplicationID = @LocalLicenseAppId 
            AND TestResult = 1";

            return await ExecuteDbOperationAsync(async () =>
                (byte)await _context.Tests.FromSqlRaw(query,
                new SqlParameter("@LocalLicenseAppId", localLicenseAppId)).CountAsync());
        }

        public async Task<bool> DoesPassTestTypeAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            const string query = @"
        SELECT TOP 1 TestResult
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseAppId 
        AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY TestAppointments.TestAppointmentID DESC";

            return await ExecuteDbOperationAsync(async () =>
                await _context.LocalLicenses.FromSqlRaw(query,
                new SqlParameter("@TestTypeID", testTypeId),
                new SqlParameter("localLicenseAppId", localLicenseAppId))
                .AnyAsync());
        }

        public async Task<bool> DoesAttendTestTypeAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            const string query = @"
        SELECT TOP 1 Found = 1
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseAppId 
        AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY TestAppointments.TestAppointmentID DESC";

            return await ExecuteDbOperationAsync(async () =>
                await _context.Tests.FromSqlRaw(query,
                new SqlParameter("@TestTypeID", testTypeId),
                new SqlParameter("@localLicenseAppId", localLicenseAppId))
                .OrderBy(x => x.TestAppointmentId)
                .AnyAsync());
        }

        public async Task<byte> TotalTrialsPerTestAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            const string query = @"
        SELECT TotalTrialsPerTest = COUNT(TestID)
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseAppId 
        AND TestAppointments.TestTypeID = @TestTypeID";

            return await ExecuteDbOperationAsync(async () =>
               (byte)await _context.Tests.FromSqlRaw(query,
               new SqlParameter("@localLicenseAppId", localLicenseAppId),
               new SqlParameter("@TestTypeID", testTypeId))
               .CountAsync());
        }

        public async Task<bool> IsThereAnActiveScheduledTestAsync(int localLicenseAppId, int testTypeId)
        {
            if (localLicenseAppId <= 0)
                throw new ArgumentException($"Local Driving License Application ID {localLicenseAppId} must be greater than 0.");

            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            const string query = @"
        SELECT TOP 1 Found = 1
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @localLicenseAppId  
        AND TestAppointments.TestTypeID = @TestTypeID 
        AND IsLocked = 0
        ORDER BY TestAppointments.TestAppointmentID DESC";

            return await ExecuteDbOperationAsync(async () =>
             await _context.LocalLicenses.FromSqlRaw(query,
             new SqlParameter("@localLicenseAppId", localLicenseAppId),
             new SqlParameter("@TestTypeID", testTypeId))
             .AnyAsync());
        }

        public async Task<int> GetTestIdByAppointmentIdAsync(int testAppointmentId)
        {
            if (testAppointmentId <= 0)
                throw new ArgumentException($"Test Appointment ID {testAppointmentId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.Tests
                .Where(x => x.TestAppointmentId == testAppointmentId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync());
        }


        //Private Helper
        private async Task LockTestAppointmentAsync(Test test)
        {
            // Lock the associated test appointment
            var testAppointment = await _context.TestAppointments
                .FirstOrDefaultAsync(x => x.Id == test.TestAppointmentId);

            if (testAppointment != null)
            {
                testAppointment.IsLocked = true;
                _context.TestAppointments.Update(testAppointment);
            }
        }
    }
}