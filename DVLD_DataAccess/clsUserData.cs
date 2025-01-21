using DVLD.DTOs;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsUserData
    {
        public static UsersDTO GetUserInfoByUserId(int userId)
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
                            return new UsersDTO
                            {
                                PersonID = (int)reader["PersonID"],
                                Username = (string)reader["UserName"],
                                Password = (string)reader["Password"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }

                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static UsersDTO GetUserInfoByPersonId(int personId)
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
                            return new UsersDTO()
                            {
                                UserID = (int)reader["UserID"],
                                Username = (string)reader["UserName"],
                                Password = (string)reader["Password"],
                                IsActive = (bool)reader["IsActive"]
                            };
                          
                        }

                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static UsersDTO GetUserInfoByUsernameAndPassword(string userName, string password)
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
                            return new UsersDTO()
                            {
                                UserID = (int)reader["UserID"],
                                PersonID = (int)reader["PersonID"],
                                Username = (string)reader["UserName"],
                                Password = (string)reader["Password"],
                                IsActive = (bool)reader["IsActive"]
                            };
                        }

                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static int AddNewUser(UsersDTO usersDTO)
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
                    command.Parameters.AddWithValue("@PersonID", usersDTO.PersonID);
                    command.Parameters.AddWithValue("@UserName", usersDTO.Username);
                    command.Parameters.AddWithValue("@Password", usersDTO.Password);
                    command.Parameters.AddWithValue("@IsActive", usersDTO.IsActive);

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

        public static bool UpdateUser(UsersDTO usersDTO)
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
                    command.Parameters.AddWithValue("@PersonID", usersDTO.PersonID);
                    command.Parameters.AddWithValue("@UserName", usersDTO.Username);
                    command.Parameters.AddWithValue("@Password", usersDTO.Password);
                    command.Parameters.AddWithValue("@IsActive", usersDTO.IsActive);
                    command.Parameters.AddWithValue("@UserID", usersDTO.UserID);

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
