namespace DVLD_DataAccess
{
    public static class LocalLicenseAppSqlStatements
    {
        public const string GetByID = @"
        SELECT * FROM LocalDrivingLicenseApplications 
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        public const string GetByApplicationId = @"
        SELECT * FROM LocalDrivingLicenseApplications 
        WHERE ApplicationID = @ApplicationID";

        public const string GetAll = @"
        SELECT * FROM LocalDrivingLicenseApplications_View 
        ORDER BY ApplicationDate DESC";

        public const string AddNew = @"
        INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
        VALUES (@ApplicationID, @LicenseClassID);
        SELECT SCOPE_IDENTITY();";

        public const string Update = @"
        UPDATE LocalDrivingLicenseApplications  
        SET ApplicationID = @ApplicationID,
            LicenseClassID = @LicenseClassID
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        public const string Delete = @"
        DELETE FROM LocalDrivingLicenseApplications 
        WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

        public const string DoesPassTestType = @"
        SELECT TOP 1 TestResult
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
        AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY TestAppointments.TestAppointmentID DESC";

        public const string DoesAttendTestType = @"
        SELECT TOP 1 Found = 1
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
        AND TestAppointments.TestTypeID = @TestTypeID
        ORDER BY TestAppointments.TestAppointmentID DESC";

        public const string TotalTrialsPerTest = @"
        SELECT TotalTrialsPerTest = COUNT(TestID)
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
        AND TestAppointments.TestTypeID = @TestTypeID";

        public const string IsThereAnActiveScheduledTest = @"
        SELECT TOP 1 Found = 1
        FROM LocalDrivingLicenseApplications 
        INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
        WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID  
        AND TestAppointments.TestTypeID = @TestTypeID 
        AND IsLocked = 0
        ORDER BY TestAppointments.TestAppointmentID DESC";
    }


}



