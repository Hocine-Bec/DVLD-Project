using DVLD.DTOs;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public class clsPersonData 
    {
        public static PersonDTO GetPersonInfoById(int personId)
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
                            return new PersonDTO
                            {
                                PersonID = personId,
                                FirstName = (string)reader["FirstName"],
                                SecondName = (string)reader["SecondName"],
                                ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"],
                                LastName = (string)reader["LastName"],
                                NationalNo = (string)reader["NationalNo"],
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = (byte)reader["Gendor"],
                                Address = (string)reader["Address"],
                                Phone = (string)reader["Phone"],
                                Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                                NationalityCountryID = (int)reader["NationalityCountryID"],
                                ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"]
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

        public static PersonDTO GetPersonInfoByNationalNo(string nationalNo)
        {
            const string query = "SELECT * FROM People WHERE NationalNo = @nationalNo";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nationalNo", nationalNo);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new PersonDTO
                            {
                                PersonID = (int)reader["PersonID"],
                                FirstName = (string)reader["FirstName"],
                                SecondName = (string)reader["SecondName"],
                                ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"],
                                LastName = (string)reader["LastName"],
                                NationalNo = nationalNo,
                                DateOfBirth = (DateTime)reader["DateOfBirth"],
                                Gender = (byte)reader["Gendor"],
                                Address = (string)reader["Address"],
                                Phone = (string)reader["Phone"],
                                Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                                NationalityCountryID = (int)reader["NationalityCountryID"],
                                ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"]
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

        public static int AddNewPerson(PersonDTO person)
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
                    command.Parameters.AddWithValue("@FirstName", person.FirstName);
                    command.Parameters.AddWithValue("@SecondName", person.SecondName);
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(person.ThirdName) ? DBNull.Value : (object)person.ThirdName);
                    command.Parameters.AddWithValue("@LastName", person.LastName);
                    command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                    command.Parameters.AddWithValue("@gender", person.Gender);
                    command.Parameters.AddWithValue("@Address", person.Address);
                    command.Parameters.AddWithValue("@Phone", person.Phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(person.Email) ? DBNull.Value : (object)person.Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", person.NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(person.Email) ? DBNull.Value : (object)person.Email);

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

        public static bool UpdatePerson(PersonDTO person)
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
                    command.Parameters.AddWithValue("@PersonID", person.PersonID);
                    command.Parameters.AddWithValue("@FirstName", person.FirstName);
                    command.Parameters.AddWithValue("@SecondName", person.SecondName);
                    command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(person.ThirdName) ? DBNull.Value : (object)person.ThirdName);
                    command.Parameters.AddWithValue("@LastName", person.LastName);
                    command.Parameters.AddWithValue("@NationalNo", person.NationalNo);
                    command.Parameters.AddWithValue("@DateOfBirth", person.DateOfBirth);
                    command.Parameters.AddWithValue("@gender", person.Gender);
                    command.Parameters.AddWithValue("@Address", person.Address);
                    command.Parameters.AddWithValue("@Phone", person.Phone);
                    command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(person.Email) ? DBNull.Value : (object)person.Email);
                    command.Parameters.AddWithValue("@NationalityCountryID", person.NationalityCountryID);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(person.Email) ? DBNull.Value : (object)person.Email);
                    command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(person.ImagePath) ? DBNull.Value : (object)person.ImagePath);

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
