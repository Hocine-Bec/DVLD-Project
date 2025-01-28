namespace DVLD_DataAccess
{
    public static class TestSqlStatements
    {
        public const string GetById = "SELECT * FROM Tests WHERE TestID = @TestID";
        public const string GetLastTestByPersonAndTestTypeAndLicenseClass = @"
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
        public const string GetAll = "SELECT * FROM Tests ORDER BY TestID";
        public const string AddNew = @"
        INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
        VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

        UPDATE TestAppointments 
        SET IsLocked = 1 
        WHERE TestAppointmentID = @TestAppointmentID;

        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE Tests  
        SET 
            TestAppointmentID = @TestAppointmentID,
            TestResult = @TestResult,
            Notes = @Notes,
            CreatedByUserID = @CreatedByUserID
        WHERE TestID = @TestID";
        public const string GetPassedTestCount = @"
        SELECT PassedTestCount = COUNT(TestTypeID)
        FROM Tests 
        INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
            AND TestResult = 1";
    }

}
