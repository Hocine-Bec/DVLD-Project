namespace DVLD_DataAccess
{
    public static class ApplicationSqlStatements
    {
        public const string GetById = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";
        public const string GetAll = "SELECT * FROM ApplicationsList_View ORDER BY ApplicationDate";
        public const string AddNew = @"
        INSERT INTO Applications 
        (
            ApplicantPersonID, ApplicationDate, ApplicationTypeID,
            ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID
        )
        VALUES 
        (
            @ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
            @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID
        );
        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE Applications  
        SET 
            ApplicantPersonID = @ApplicantPersonID,
            ApplicationDate = @ApplicationDate,
            ApplicationTypeID = @ApplicationTypeID,
            ApplicationStatus = @ApplicationStatus, 
            LastStatusDate = @LastStatusDate,
            PaidFees = @PaidFees,
            CreatedByUserID = @CreatedByUserID
        WHERE ApplicationID = @ApplicationID";
        public const string Delete = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";
        public const string IsExist = "SELECT Found = 1 FROM Applications WHERE ApplicationID = @ApplicationID";
        public const string GetActiveApplicationId = @"
        SELECT ApplicationID 
        FROM Applications 
        WHERE ApplicantPersonID = @ApplicantPersonID 
            AND ApplicationTypeID = @ApplicationTypeID 
            AND ApplicationStatus = 1";
        public const string GetActiveApplicationIdForLicenseClass = @"
        SELECT Applications.ApplicationID  
        FROM Applications 
        INNER JOIN LocalDrivingLicenseApplications 
            ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
        WHERE ApplicantPersonID = @ApplicantPersonID 
            AND ApplicationTypeID = @ApplicationTypeID 
            AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
            AND ApplicationStatus = 1";
        public const string UpdateStatus = @"
        UPDATE Applications  
        SET 
            ApplicationStatus = @NewStatus, 
            LastStatusDate = @LastStatusDate
        WHERE ApplicationID = @ApplicationID";
    }


}
