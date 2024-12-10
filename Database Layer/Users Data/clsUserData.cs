using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database_Layer
{
    public static class clsUserDataLayer
    {
        public static DataTable GetUsersList()
        {
            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            var d1 = new DataTable();

            string query = "SELECT UserID, People.PersonID, " +
                "FullName = (People.FirstName + ' ' + People.SecondName + ' ' + People.ThirdName + ' ' + People.LastName), " +
                "UserName, IsActive " +
                "FROM Users " +
                "INNER JOIN People ON People.PersonID = Users.PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    d1.Load(reader);
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return d1;
        }


        public static int AddNewUser(int PersonID, string Username, string Password, bool IsActive)
        {
            int UserID = -1;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "INSERT INTO Users (PersonID, UserName, Password, IsActive) " +
                "VALUES (@PersonID, @Username, @Password, @IsActive);" +
                "SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);
            cmd.Parameters.AddWithValue("@UserName", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();

                object obj = cmd.ExecuteScalar();

                if (obj != null && int.TryParse(obj.ToString(), out int Id))
                {
                    UserID = Id;
                }

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return UserID;
        }


        public static bool FindUserWithUserID(ref int UserID, ref int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

                    IsFound = true;
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool FindUserWithNationalNo(string NationalNo, ref int UserID, ref int PersonID, ref string Username,
            ref string Password, ref bool IsActive)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query =
                "SELECT Users.* " +
                "FROM Users " +
                "INNER JOIN People On Users.PersonID = People.PersonID " +
                "WHERE People.NationalNo = @NationalNo;";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

                    IsFound = true;
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool FindUsersWithFilters(string Filter, string name, ref int UserID, ref int PersonID,
            ref string Username, ref string Password, ref bool IsActive)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM Users WHERE @Filter = @name";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@Filter", Filter);
            cmd.Parameters.AddWithValue("@name", name);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

                    IsFound = true;
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool UpdateUser(int UserID, string Username, string Password, bool IsActive)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);
            string query = "Update Users " +
                "Set Username = @Username, " +
                "Password = @Password, " +
                "IsActive = @IsActive " +
                "Where UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@IsActive", IsActive);

            try
            {
                connection.Open();

                int row = cmd.ExecuteNonQuery();

                if (row > 0)
                {
                    IsFound = true;
                }

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool Delete(int UserID)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "DELETE FROM Users Where UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                int row = cmd.ExecuteNonQuery();

                if (row > 0)
                {
                    IsFound = true;
                }

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM Users WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;

        }

        public static bool IsUser(int PersonID)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM Users " +
                "INNER JOIN People ON People.PersonID = Users.PersonID " +
                "WHERE Users.PersonID = @PersonID;";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool FindUserWithPersonID(ref int UserID, ref int PersonID, ref string Username, ref string Password, ref bool IsActive)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM Users " +
                "INNER JOIN People ON People.PersonID = Users.PersonID " +
                "WHERE Users.PersonID = @PersonID;";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    PersonID = (int)reader["PersonID"];
                    Username = (string)reader["Username"];
                    Password = (string)reader["Password"];
                    IsActive = (bool)reader["IsActive"];

                    IsFound = true;
                }

                reader.Close();
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return IsFound;
        }

        public static bool IsCorrectPassword(int ID, string password)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT Password FROM Users " +
                           "WHERE UserID = @UserID;";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    isFound = ((string)reader["Password"] == password);
                }

                reader.Close();
            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool UpdatePassword(int ID, string NewPassword)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "UPDATE Users " +
                           "SET Users.Password = @NewPassword " +
                           "WHERE UserID = @UserID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@UserID", ID);
            cmd.Parameters.AddWithValue("@NewPassword", NewPassword);

            try
            {
                connection.Open();

                int row = cmd.ExecuteNonQuery();

                if (row > 0)
                {
                    isFound = true;
                }

            }
            catch
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
