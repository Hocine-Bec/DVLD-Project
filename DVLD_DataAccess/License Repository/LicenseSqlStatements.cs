namespace DVLD_DataAccess
{
    public static class LicenseSqlStatements
    {
        public const string GetById = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
        public const string GetAll = "SELECT * FROM Licenses";
        public const string GetDriverLicenses = @"
        SELECT     
            Licenses.LicenseID,
            ApplicationID,
            LicenseClasses.ClassName, 
            Licenses.IssueDate, 
            Licenses.ExpirationDate, 
            Licenses.IsActive
        FROM Licenses 
        INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
        WHERE DriverID = @DriverID
        ORDER BY IsActive DESC, ExpirationDate DESC";
        public const string AddNew = @"
        INSERT INTO Licenses
        (
            ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate,
            Notes, PaidFees, IsActive, IssueReason, CreatedByUserID
        )
        VALUES
        (
            @ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate,
            @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID
        );
        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE Licenses
        SET 
            ApplicationID = @ApplicationID, 
            DriverID = @DriverID,
            LicenseClass = @LicenseClass,
            IssueDate = @IssueDate,
            ExpirationDate = @ExpirationDate,
            Notes = @Notes,
            PaidFees = @PaidFees,
            IsActive = @IsActive,
            IssueReason = @IssueReason,
            CreatedByUserID = @CreatedByUserID
        WHERE LicenseID = @LicenseID";
        public const string GetActiveLicenseIdByPersonId = @"
        SELECT Licenses.LicenseID
        FROM Licenses 
        INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
        WHERE 
            Licenses.LicenseClass = @LicenseClass 
            AND Drivers.PersonID = @PersonID
            AND IsActive = 1;";
        public const string Deactivate = @"
        UPDATE Licenses
        SET IsActive = 0
        WHERE LicenseID = @LicenseID";
    }
}
