namespace DVLD_DataAccess
{
    public static class AppTypeSqlStatements
    {
        public const string GetById = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";
        public const string GetAllApplicationTypes = "SELECT * FROM ApplicationTypes ORDER BY ApplicationTypeTitle";
        public const string AddNewApplicationType = @"
        INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
        VALUES (@Title, @Fees);
        SELECT SCOPE_IDENTITY();";
        public const string UpdateApplicationType = @"
        UPDATE ApplicationTypes  
        SET 
            ApplicationTypeTitle = @Title,
            ApplicationFees = @Fees
        WHERE ApplicationTypeID = @ApplicationTypeID";
    }

}
