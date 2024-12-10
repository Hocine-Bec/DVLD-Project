using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Database_Layer
{
    public class clsPersonInfo
    {
        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }
    }

    public static class clsPersonDataLayer
    {
        public static DataTable GetCountriesList()
        {
            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            var d1 = new DataTable();

            string query = "SELECT * FROM Countries";

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

        public static DataTable GetPeopleList()
        {
            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            var d1 = new DataTable();

            string query = "SELECT * FROM People";

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


        public static int AddNewPerson(string FirstName, string SecondName, string ThirdName, string LastName,
        string NationalNo, DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email, 
        int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "INSERT INTO People (FirstName, SecondName, ThirdName, LastName, " +
                "NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath) " +
                "VALUES (@FirstName, @SecondName, @ThirdName, @LastName, @NationalNo, @DateOfBirth, @Gendor, " +
                "@Address, @Phone, @Email, @NationalityCountryID, @ImagePath);" +
                "SELECT SCOPE_IDENTITY();";



            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@FirstName",  FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@ThirdName",  ThirdName);
            cmd.Parameters.AddWithValue("@LastName",   LastName);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@DateOfBirth",DateOfBirth);
            cmd.Parameters.AddWithValue("@Gendor",     Gendor);
            cmd.Parameters.AddWithValue("@Address",    Address);
            cmd.Parameters.AddWithValue("@Phone",      Phone);
            cmd.Parameters.AddWithValue("@Email",      Email);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != null && ImagePath != "")
            {
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            }


            try
            {
                connection.Open();

                object obj = cmd.ExecuteScalar();

                if (obj != null && int.TryParse(obj.ToString(), out int Id))
                {
                    PersonID = Id;
                }
                
            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }

        
        public static bool Find(int ID, ref clsPersonInfo person)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    person.PersonID = (int)reader["PersonID"];
                    person.FirstName = (string)reader["FirstName"];
                    person.SecondName = (string)reader["SecondName"];
                    person.ThirdName = (string)reader["ThirdName"];
                    person.LastName = (string)reader["LastName"];
                    person.NationalNo = (string)reader["NationalNo"];
                    person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    person.Gendor = Convert.ToInt32(reader["Gendor"]);
                    person.Address = (string)reader["Address"];
                    person.Phone = (string)reader["Phone"];
                    person.Email = (string)reader["Email"];
                    person.NationalityCountryID = (int)reader["NationalityCountryID"];
                    person.ImagePath = (reader["ImagePath"] == DBNull.Value || (string)reader["ImagePath"] == "") ? "" : (string)reader["ImagePath"];
                  

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

        public static bool Find(string name, ref clsPersonInfo person)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@NationalNo", name);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    person.PersonID = (int)(reader["PersonID"]);
                    person.FirstName = (string)reader["FirstName"];
                    person.SecondName = (string)reader["SecondName"];
                    person.ThirdName = (string)reader["ThirdName"];
                    person.LastName = (string)reader["LastName"];
                    person.NationalNo = (string)reader["NationalNo"];
                    person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    person.Gendor = Convert.ToInt32(reader["Gendor"]);
                    person.Address = (string)reader["Address"];
                    person.Phone = (string)reader["Phone"];
                    person.Email = (string)reader["Email"];
                    person.NationalityCountryID = (int)reader["NationalityCountryID"];
                    person.ImagePath = (reader["ImagePath"] == DBNull.Value || (string)reader["ImagePath"] == "") ? "" : (string)reader["ImagePath"];


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


        public static bool Find(string Filter, string name, ref clsPersonInfo person)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM People WHERE @Filter = @name";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@Filter", Filter);
            cmd.Parameters.AddWithValue("@name", name);

            try
            {
                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    person.PersonID = (int)reader["PersonID"];
                    person.FirstName = (string)reader["FirstName"];
                    person.SecondName = (string)reader["SecondName"];
                    person.ThirdName = (string)reader["ThirdName"];
                    person.LastName = (string)reader["LastName"];
                    person.NationalNo = (string)reader["NationalNo"];
                    person.DateOfBirth = (DateTime)reader["DateOfBirth"];
                    person.Gendor = Convert.ToInt32(reader["Gendor"]);
                    person.Address = (string)reader["Address"];
                    person.Phone = (string)reader["Phone"];
                    person.Email = (string)reader["Email"];
                    person.NationalityCountryID = (int)reader["NationalityCountryID"];
                    person.ImagePath = (reader["ImagePath"] == DBNull.Value || (string)reader["ImagePath"] == "") ? "" : (string)reader["ImagePath"];


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


        public static bool Update(int ID, string FirstName, string SecondName, string ThirdName, string LastName,
        string NationalNo, DateTime DateOfBirth, int Gendor, string Address, string Phone, string Email,
        int NationalityCountryID, string ImagePath)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);
            string query = "Update People " +
                "Set FirstName = @FirstName, " +
                "SecondName = @SecondName, " +
                "ThirdName = @ThirdName, " +
                "LastName = @LastName, " +
                "NationalNo = @NationalNo, " +
                "DateOfBirth = @DateOfBirth, " +
                "Gendor = @Gendor, " +
                "Address = @Address, " +
                "Phone = @Phone, " +
                "Email = @Email, " +
                "NationalityCountryID = @NationalityCountryID, " +
                "ImagePath = @ImagePath " +
                "Where PersonID = @PersonID";



            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", ID);
            cmd.Parameters.AddWithValue("@FirstName", FirstName);
            cmd.Parameters.AddWithValue("@SecondName", SecondName);
            cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
            cmd.Parameters.AddWithValue("@LastName", LastName);
            cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
            cmd.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            cmd.Parameters.AddWithValue("@Gendor", Gendor);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (ImagePath != null && ImagePath != "")
            {
                cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ImagePath", DBNull.Value);
            }

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

        public static bool Delete(int PersonID)
        {

            bool IsFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "DELETE FROM People Where PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", PersonID);
           
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


        public static bool IsPersonExist(int ID)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM People WHERE PersonID = @PersonID";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@PersonID", ID);

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
        public static bool IsPersonExist(string No)
        {
            bool isFound = false;

            var connection = new SqlConnection(DataConnectionLink.ConnectionString);

            string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            SqlCommand cmd = new SqlCommand(query, connection);

            cmd.Parameters.AddWithValue("@NationalNo", No);

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


    }


}


