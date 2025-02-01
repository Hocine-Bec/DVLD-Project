namespace DVLD_DataAccess
{
    public static class DetainedSqlStatements
    {
        public const string GetById = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";
        public const string GetByLicenseId = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID ORDER BY DetainID DESC";
        public const string GetAll = "SELECT * FROM DetainedLicenses_View ORDER BY IsReleased, DetainID";
        public const string AddNew = @"
        INSERT INTO dbo.DetainedLicenses
        (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
        VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE dbo.DetainedLicenses
        SET LicenseID = @LicenseID, 
            DetainDate = @DetainDate, 
            FineFees = @FineFees,
            CreatedByUserID = @CreatedByUserID
        WHERE DetainID = @DetainID;";
        public const string Release = @"
        UPDATE dbo.DetainedLicenses
        SET IsReleased = 1, 
            ReleaseDate = @ReleaseDate, 
            ReleasedByUserID = @ReleasedByUserID,
            ReleaseApplicationID = @ReleaseApplicationID
        WHERE DetainID = @DetainID;";
        public const string IsLicenseDetained = @"
        SELECT IsDetained = 1 FROM DetainedLicenses 
        WHERE LicenseID = @LicenseID AND IsReleased = 0;";
    }


}
