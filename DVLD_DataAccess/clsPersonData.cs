using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
        public static bool GetPersonInfoById(int personId, ref string firstName, ref string secondName,
           ref string thirdName, ref string lastName, ref string nationalNo, ref DateTime dateOfBirth,
           ref short gender, ref string address, ref string phone, ref string email,
           ref int nationalityCountryId, ref string imagePath)
        {
            const string query = "SELECT * FROM People WHERE PersonID = @PersonID";

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
                            firstName = (string)reader["FirstName"];
                            secondName = (string)reader["SecondName"];
                            thirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            lastName = (string)reader["LastName"];
                            nationalNo = (string)reader["NationalNo"];
                            dateOfBirth = (DateTime)reader["DateOfBirth"];
                            gender = (byte)reader["Gendor"];
                            address = (string)reader["Address"];
                            phone = (string)reader["Phone"];
                            email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                            nationalityCountryId = (int)reader["NationalityCountryID"];
                            imagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
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

        public static bool GetPersonInfoByNationalNo(string nationalNo, ref int personId, ref string firstName,
            ref string secondName, ref string thirdName, ref string lastName, ref DateTime dateOfBirth,
            ref short gender, ref string address, ref string phone, ref string email,
            ref int nationalityCountryId, ref string imagePath)
        {
            const string query = "SELECT * FROM People WHERE NationalNo = @NationalNo";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personId = (int)reader["PersonID"];
                            firstName = (string)reader["FirstName"];
                            secondName = (string)reader["SecondName"];
                            thirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"];
                            lastName = (string)reader["LastName"];
                            dateOfBirth = (DateTime)reader["DateOfBirth"];
                            gender = (byte)reader["Gendor"];
                            address = (string)reader["Address"];
                            phone = (string)reader["Phone"];
                            email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"];
                            nationalityCountryId = (int)reader["NationalityCountryID"];
                            imagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"];
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

        public static int AddNewPerson(string firstName, string secondName, string thirdName, string lastName,
            string nationalNo, DateTime dateOfBirth, short gender, string address, string phone, string email,
            int nationalityCountryId, string imagePath)
        {
            const string query = @"
            INSERT INTO People 
            (
                FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth, Gendor, 
                Address, Phone, Email, NationalityCountryID, ImagePath
            )
            VALUES 
            (
                @FirstName, @SecondName, @ThirdName, @LastName, @NationalNo, @DateOfBirth, @gender, 
                @Address, @Phone, @Email, @NationalityCountryID, @ImagePath
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@SecondName", secondName);
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(thirdName) ? DBNull.Value : (object)thirdName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@gender", gender);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);
                    command.Parameters.AddWithValue("@NationalityCountryID", nationalityCountryId);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(imagePath) ? DBNull.Value : (object)imagePath);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var personId)
                        ? personId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdatePerson(int personId, string firstName, string secondName, string thirdName,
            string lastName, string nationalNo, DateTime dateOfBirth, short gender, string address, string phone,
            string email, int nationalityCountryId, string imagePath)
        {
            const string query = @"
            UPDATE People  
            SET 
                FirstName = @FirstName,
                SecondName = @SecondName,
                ThirdName = @ThirdName,
                LastName = @LastName, 
                NationalNo = @NationalNo,
                DateOfBirth = @DateOfBirth,
                Gendor = @Gendor,
                Address = @Address,  
                Phone = @Phone,
                Email = @Email, 
                NationalityCountryID = @NationalityCountryID,
                ImagePath = @ImagePath
            WHERE PersonID = @PersonID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@SecondName", secondName);
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(thirdName) ? DBNull.Value : (object)thirdName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
                    command.Parameters.AddWithValue("@Gendor", gender);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Phone", phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(email) ? DBNull.Value : (object)email);
                    command.Parameters.AddWithValue("@NationalityCountryID", nationalityCountryId);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(imagePath) ? DBNull.Value : (object)imagePath);

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

        public static DataTable GetAllPeople()
        {
            const string query = @"
            SELECT 
                People.PersonID, People.NationalNo,
                People.FirstName, People.SecondName, People.ThirdName, People.LastName,
                People.DateOfBirth, People.Gendor,  
                CASE
                    WHEN People.Gendor = 0 THEN 'Male'
                    ELSE 'Female'
                END AS GendorCaption,
                People.Address, People.Phone, People.Email, 
                People.NationalityCountryID, Countries.CountryName, People.ImagePath
            FROM People 
            INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID
            ORDER BY People.FirstName";

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

        public static bool DeletePerson(int personId)
        {
            const string query = "DELETE FROM People WHERE PersonID = @PersonID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

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

        public static bool IsPersonExist(int personId)
        {
            const string query = "SELECT Found = 1 FROM People WHERE PersonID = @PersonID";

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

        public static bool IsPersonExist(string nationalNo)
        {
            const string query = "SELECT Found = 1 FROM People WHERE NationalNo = @NationalNo";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);

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

    }

}
