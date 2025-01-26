namespace DVLD_DataAccess
{
    public static class TestTypeSqlStatements
    {
        public const string GetById = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";
        public const string GetByTitle = "SELECT * FROM TestTypes WHERE TestTypeTitle = @TestTypeTitle";
        public const string GetAllTestTypes = "SELECT * FROM TestTypes ORDER BY TestTypeID";
        public const string AddNewTestType = @"
        INSERT INTO TestTypes 
        (
            TestTypeTitle, TestTypeDescription, TestTypeFees
        )
        VALUES 
        (
            @TestTypeTitle, @TestTypeDescription, @TestTypeFees
        );
        SELECT SCOPE_IDENTITY();";
        public const string UpdateTestType = @"
        UPDATE TestTypes  
        SET 
            TestTypeTitle = @TestTypeTitle,
            TestTypeDescription = @TestTypeDescription,
            TestTypeFees = @TestTypeFees
        WHERE TestTypeID = @TestTypeID";
    }

}
