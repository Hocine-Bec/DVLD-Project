using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsUserData
    {
        public static bool GetUserInfoByUserId(int userId, ref int personId, ref string userName,
           ref string password, ref bool isActive)
        {
            const string query = "SELECT * FROM Users WHERE UserID = @UserID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personId = (int)reader["PersonID"];
                            userName = (string)reader["UserName"];
                            password = (string)reader["Password"];
                            isActive = (bool)reader["IsActive"];
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool GetUserInfoByPersonId(int personId, ref int userId, ref string userName,
            ref string password, ref bool isActive)
        {
            const string query = "SELECT * FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = (int)reader["UserID"];
                            userName = (string)reader["UserName"];
                            password = (string)reader["Password"];
                            isActive = (bool)reader["IsActive"];
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool GetUserInfoByUsernameAndPassword(string userName, string password,
            ref int userId, ref int personId, ref bool isActive)
        {
            const string query = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = (int)reader["UserID"];
                            personId = (int)reader["PersonID"];
                            userName = (string)reader["UserName"];
                            password = (string)reader["Password"];
                            isActive = (bool)reader["IsActive"];
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static int AddNewUser(int personId, string userName, string password, bool isActive)
        {
            const string query = @"
            INSERT INTO Users (PersonID, UserName, Password, IsActive)
            VALUES (@PersonID, @UserName, @Password, @IsActive);
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@IsActive", isActive);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var userId)
                        ? userId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateUser(int userId, int personId, string userName, string password, bool isActive)
        {
            const string query = @"
            UPDATE Users  
            SET 
                PersonID = @PersonID,
                UserName = @UserName,
                Password = @Password,
                IsActive = @IsActive
            WHERE UserID = @UserID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@UserName", userName);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable GetAllUsers()
        {
            const string query = @"
            SELECT 
                Users.UserID, Users.PersonID,
                FullName = People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName,
                Users.UserName, Users.IsActive
            FROM Users 
            INNER JOIN People ON Users.PersonID = People.PersonID";

            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                dataTable.Clear();
            }

            return dataTable;
        }

        public static bool DeleteUser(int userId)
        {
            const string query = "DELETE FROM Users WHERE UserID = @UserID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsUserExist(int userId)
        {
            const string query = "SELECT Found = 1 FROM Users WHERE UserID = @UserID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsUserExist(string userName)
        {
            const string query = "SELECT Found = 1 FROM Users WHERE UserName = @UserName";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool IsUserExistForPersonId(int personId)
        {
            const string query = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool DoesPersonHaveUser(int personId)
        {
            const string query = "SELECT Found = 1 FROM Users WHERE PersonID = @PersonID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool ChangePassword(int userId, string newPassword)
        {
            const string query = @"
            UPDATE Users  
            SET Password = @Password
            WHERE UserID = @UserID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);
                    command.Parameters.AddWithValue("@Password", newPassword);

                    connection.Open();
                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }

    }

}
