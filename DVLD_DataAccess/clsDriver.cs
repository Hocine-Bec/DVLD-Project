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
    public class clsDriverData
    {
        public static bool GetDriverInfoByDriverId(int driverId, ref int personId, ref int createdByUserId,
                   ref DateTime createdDate)
        {
            const string query = "SELECT * FROM Drivers WHERE DriverID = @DriverID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            personId = (int)reader["PersonID"];
                            createdByUserId = (int)reader["CreatedByUserID"];
                            createdDate = (DateTime)reader["CreatedDate"];
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

        public static bool GetDriverInfoByPersonId(int personId, ref int driverId,
            ref int createdByUserId, ref DateTime createdDate)
        {
            const string query = "SELECT * FROM Drivers WHERE PersonID = @PersonID";

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
                            driverId = (int)reader["DriverID"];
                            createdByUserId = (int)reader["CreatedByUserID"];
                            createdDate = (DateTime)reader["CreatedDate"];
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

        public static DataTable GetAllDrivers()
        {
            const string query = "SELECT * FROM Drivers_View ORDER BY FullName";
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

        public static int AddNewDriver(int personId, int createdByUserId)
        {
            const string query = @"INSERT INTO Drivers (PersonID, CreatedByUserID, CreatedDate)
                             VALUES (@PersonID, @CreatedByUserID, @CreatedDate);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
                    command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

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

        public static bool UpdateDriver(int driverId, int personId, int createdByUserId)
        {
            const string query = @"UPDATE Drivers  
                            SET PersonID = @PersonID,
                            CreatedByUserID = @CreatedByUserID
                            WHERE DriverID = @DriverID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);

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
