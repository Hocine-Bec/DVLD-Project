namespace DVLD_DataAccess
{
    public static class TestAppointmentSqlStatements
    {
        public const string GetById = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";
        public const string GetLastTestAppointment = @"
        SELECT TOP 1 *
        FROM TestAppointments
        WHERE TestTypeID = @TestTypeID 
            AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
        ORDER BY TestAppointmentID DESC";
        public const string GetAll = "SELECT * FROM TestAppointments_View ORDER BY AppointmentDate DESC";
        public const string GetApplicationTestAppointmentsPerTestType = @"
        SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
        FROM TestAppointments
        WHERE TestTypeID = @TestTypeID 
            AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
        ORDER BY TestAppointmentID DESC";
        public const string AddNew = @"
        INSERT INTO TestAppointments 
        (
            TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, 
            PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID
        )
        VALUES 
        (
            @TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, 
            @PaidFees, @CreatedByUserID, 0, @RetakeTestApplicationID
        );
        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE TestAppointments  
        SET 
            TestTypeID = @TestTypeID,
            LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
            AppointmentDate = @AppointmentDate,
            PaidFees = @PaidFees,
            CreatedByUserID = @CreatedByUserID,
            IsLocked = @IsLocked,
            RetakeTestApplicationID = @RetakeTestApplicationID
        WHERE TestAppointmentID = @TestAppointmentID";
        public const string GetTestId = "SELECT TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";
    }
}
