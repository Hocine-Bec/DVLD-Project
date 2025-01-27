using DVLD.DTOs;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class UsersRepository
    {
        public UsersDTO GetUserInfoByUserId(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.GetByUserId, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return UserDataMapper.MapToUserDTO(reader);
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

        public UsersDTO GetUserInfoByPersonId(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.GetByPersonId, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return UserDataMapper.MapToUserDTO(reader);
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

        public UsersDTO GetUserInfoByUsernameAndPassword(string userName, string password)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.GetByUsernameAndPassword, connection))
                {
                    command.Parameters.AddWithValue("@Username", userName);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return UserDataMapper.MapToUserDTO(reader);
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

        public int AddNewUser(UsersDTO usersDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.AddNewUser, connection))
                {
                    UserParameterBuilder.FillSqlCommandParameters(command, usersDTO);

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

        public bool UpdateUser(UsersDTO usersDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.UpdateUser, connection))
                {
                    command.Parameters.AddWithValue("@UserID", usersDTO.UserID);
                    UserParameterBuilder.FillSqlCommandParameters(command, usersDTO);

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

        public DataTable GetAllUsers()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.GetAllUsers, connection))
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

        public bool DeleteUser(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.DeleteUser, connection))
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

        public bool IsUserExist(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.IsUserExistById, connection))
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

        public bool IsUserExist(string userName)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.IsUserExistByUsername, connection))
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

        public bool IsUserExistByPersonId(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.IsUserExistByPersonId, connection))
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

        public bool DoesPersonHaveUser(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.IsPersonHaveUser, connection))
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

        public bool ChangePassword(int userId, string newPassword)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(UserSqlStatements.ChangePassword, connection))
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
