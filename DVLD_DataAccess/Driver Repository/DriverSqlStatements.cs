namespace DVLD_DataAccess
{
    public static class DriverSqlStatements
    {
        public const string GetByDriverId = "SELECT * FROM Drivers WHERE DriverID = @DriverID";
        public const string GetByPersonId = "SELECT * FROM Drivers WHERE PersonID = @PersonID";
        public const string GetAll = "SELECT * FROM Drivers_View ORDER BY FullName";
        public const string AddNew = @"
        INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
        VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
        SELECT SCOPE_IDENTITY();";
        public const string Update = @"
        UPDATE Drivers  
        SET PersonID = @PersonID,
            CreatedByUserID = @CreatedByUserID
        WHERE DriverID = @DriverID";
    }

    

}
