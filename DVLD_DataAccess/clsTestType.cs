using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
        public static TestTypesDTO GetTestTypeInfoById(int testTypeId)
        {
            const string query = "SELECT * FROM TestTypes WHERE TestTypeID = @TestTypeID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TestTypesDTO()
                            {
                                TestTypeID = testTypeId,
                                Title = (string)reader["TestTypeTitle"],
                                Description = (string)reader["TestTypeDescription"],
                                Fees = Convert.ToSingle(reader["TestTypeFees"]),
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

        public static DataTable GetAllTestTypes()
        {
            const string query = "SELECT * FROM TestTypes ORDER BY TestTypeID";
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

        public static int AddNewTestType(TestTypesDTO testTypesDTO)
        {
            const string query = @"
            INSERT INTO TestTypes (TestTypeTitle, TestTypeDescription, TestTypeFees)
            VALUES (@TestTypeTitle, @TestTypeDescription, @TestTypeFees);
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeTitle", testTypesDTO.Title);
                    command.Parameters.AddWithValue("@TestTypeDescription", testTypesDTO.Description);
                    command.Parameters.AddWithValue("@TestTypeFees", testTypesDTO.Fees);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var testTypeId)
                        ? testTypeId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateTestType(TestTypesDTO testTypesDTO)
        {
            const string query = @"
            UPDATE TestTypes  
            SET 
                TestTypeTitle = @TestTypeTitle,
                TestTypeDescription = @TestTypeDescription,
                TestTypeFees = @TestTypeFees
            WHERE TestTypeID = @TestTypeID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", testTypesDTO.TestTypeID);
                    command.Parameters.AddWithValue("@TestTypeTitle", testTypesDTO.Title);
                    command.Parameters.AddWithValue("@TestTypeDescription", testTypesDTO.Description);
                    command.Parameters.AddWithValue("@TestTypeFees", testTypesDTO.Fees);

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
