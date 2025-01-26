namespace DVLD_DataAccess
{
    public static class LicenseClassSqlStatements
    {
        public const string GetById = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";
        public const string GetByClassName = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";
        public const string GetAllLicenseClasses = "SELECT * FROM LicenseClasses ORDER BY ClassName";
        public const string AddNewLicenseClass = @"
            INSERT INTO LicenseClasses 
            (
                ClassName, ClassDescription, MinimumAllowedAge, 
                DefaultValidityLength, ClassFees
            )
            VALUES 
            (
                @ClassName, @ClassDescription, @MinimumAllowedAge, 
                @DefaultValidityLength, @ClassFees
            );
            SELECT SCOPE_IDENTITY();";
        public const string UpdateLicenseClass = @"
            UPDATE LicenseClasses  
            SET 
                ClassName = @ClassName,
                ClassDescription = @ClassDescription,
                MinimumAllowedAge = @MinimumAllowedAge,
                DefaultValidityLength = @DefaultValidityLength,
                ClassFees = @ClassFees
            WHERE LicenseClassID = @LicenseClassID";

    }
}
