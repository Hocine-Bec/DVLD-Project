namespace DVLD_DataAccess
{
    public static class UserSqlStatements
    {
        public const string GetByUserId = "SELECT * FROM Users WHERE UserID = @UserID";

        public const string GetByPersonId = "SELECT * FROM Users WHERE PersonID = @PersonID";

        public const string GetByUsernameAndPassword = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

        public const string AddNewUser = @"
            INSERT INTO Users (PersonID, UserName, Password, IsActive)
            VALUES (@PersonID, @UserName, @Password, @IsActive);
            SELECT SCOPE_IDENTITY();";

        public const string UpdateUser = @"
            UPDATE Users  
            SET 
                PersonID = @PersonID,
                UserName = @UserName,
                Password = @Password,
                IsActive = @IsActive
            WHERE UserID = @UserID";

        public const string GetAllUsers = @"
            SELECT 
                Users.UserID, Users.PersonID,
                FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName,
                Users.UserName, Users.IsActive
            FROM Users 
            INNER JOIN People ON Users.PersonID = People.PersonID";

        public const string DeleteUser = "DELETE FROM Users WHERE UserID = @UserID";

        public const string IsUserExistById = "SELECT Found = 1 FROM Users WHERE UserID = @UserID";

        public const string IsUserExistByUsername = "SELECT Found = 1 FROM Users WHERE UserName = @UserName";

        public const string IsUserExistByPersonId = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

        public const string IsPersonHaveUser = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

        public const string ChangePassword = @"
            UPDATE Users  
            SET Password = @Password
            WHERE UserID = @UserID";
    }
}
