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

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {
        public static bool GetTestTypeInfoById(int testTypeId, ref string testTypeTitle,
                  ref string testDescription, ref float testFees)
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
                            testTypeTitle = (string)reader["TestTypeTitle"];
                            testDescription = (string)reader["TestTypeDescription"];
                            testFees = Convert.ToSingle(reader["TestTypeFees"]);
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

        public static int AddNewTestType(string title, string description, float fees)
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
                    command.Parameters.AddWithValue("@TestTypeTitle", title);
                    command.Parameters.AddWithValue("@TestTypeDescription", description);
                    command.Parameters.AddWithValue("@TestTypeFees", fees);

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

        public static bool UpdateTestType(int testTypeId, string title, string description, float fees)
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
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    command.Parameters.AddWithValue("@TestTypeTitle", title);
                    command.Parameters.AddWithValue("@TestTypeDescription", description);
                    command.Parameters.AddWithValue("@TestTypeFees", fees);

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
