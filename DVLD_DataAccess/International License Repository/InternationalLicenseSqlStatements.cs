namespace DVLD_DataAccess
{
    public static class InternationalLicenseSqlStatements
    {
        public const string GetById = @"
        SELECT * FROM InternationalLicenses 
        WHERE InternationalLicenseID = @InternationalLicenseID";

        public const string GetAll = @"
        SELECT InternationalLicenseID, ApplicationID, DriverID,
               IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
        FROM InternationalLicenses
        ORDER BY IsActive, ExpirationDate DESC";

        public const string GetDriverInternationalLicenses = @"
        SELECT InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID, 
               IssueDate, ExpirationDate, IsActive 
        FROM InternationalLicenses 
        WHERE DriverID = @DriverID 
        ORDER BY ExpirationDate DESC";

        public const string AddNew = @"
        UPDATE InternationalLicenses
        SET IsActive = 0
        WHERE DriverID = @DriverID;

        INSERT INTO InternationalLicenses
        (
            ApplicationID, DriverID, IssuedUsingLocalLicenseID,
            IssueDate, ExpirationDate, IsActive, CreatedByUserID
        )
        VALUES
        (
            @ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,
            @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID
        );
        SELECT SCOPE_IDENTITY();";

        public const string Update = @"
        UPDATE InternationalLicenses
        SET 
            ApplicationID = @ApplicationID,
            DriverID = @DriverID,
            IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
            IssueDate = @IssueDate,
            ExpirationDate = @ExpirationDate,
            IsActive = @IsActive,
            CreatedByUserID = @CreatedByUserID
        WHERE InternationalLicenseID = @InternationalLicenseID";

        public const string GetActiveInternationalLicenseIdByDriverId = @"
        SELECT TOP 1 InternationalLicenseID
        FROM InternationalLicenses
        WHERE DriverID = @DriverID AND GETDATE() BETWEEN IssueDate AND ExpirationDate
        ORDER BY ExpirationDate DESC";
    }

}
