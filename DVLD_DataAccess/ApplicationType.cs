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
    public class clsApplicationTypeData
    {

        public static bool GetApplicationTypeInfoById(int applicationTypeId,
           ref string applicationTypeTitle, ref float applicationFees)
        {
            const string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            applicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                            applicationFees = Convert.ToSingle(reader["ApplicationFees"]);
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

        public static DataTable GetAllApplicationTypes()
        {
            const string query = "SELECT * FROM ApplicationTypes ORDER BY ApplicationTypeTitle";
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

        public static int AddNewApplicationType(string title, float fees)
        {
            const string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees)
                             VALUES (@Title, @Fees);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Fees", fees);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var insertedId)
                        ? insertedId : -1;

                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateApplicationType(int applicationTypeId, string title, float fees)
        {
            const string query = @"UPDATE ApplicationTypes  
                            SET ApplicationTypeTitle = @Title,
                                ApplicationFees = @Fees
                            WHERE ApplicationTypeID = @ApplicationTypeID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Fees", fees);

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
