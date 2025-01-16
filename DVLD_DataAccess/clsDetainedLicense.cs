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
    public class clsDetainedLicenseData
    {
        public static bool GetDetainedLicenseInfoById(int detainId, ref int licenseId, ref DateTime detainDate,
          ref float fineFees, ref int createdByUserId, ref bool isReleased, ref DateTime releaseDate,
          ref int releasedByUserId, ref int releaseApplicationId)
        {
            const string query = "SELECT * FROM DetainedLicenses WHERE DetainID = @DetainID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", detainId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            licenseId = (int)reader["LicenseID"];
                            detainDate = (DateTime)reader["DetainDate"];
                            fineFees = Convert.ToSingle(reader["FineFees"]);
                            createdByUserId = (int)reader["CreatedByUserID"];
                            isReleased = (bool)reader["IsReleased"];

                            releaseDate = reader["ReleaseDate"] == DBNull.Value
                                ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];

                            releasedByUserId = reader["ReleasedByUserID"] == DBNull.Value
                                ? -1 : (int)reader["ReleasedByUserID"];

                            releaseApplicationId = reader["ReleaseApplicationID"] == DBNull.Value
                                ? -1 : (int)reader["ReleaseApplicationID"];

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

        public static bool GetDetainedLicenseInfoByLicenseId(int licenseId, ref int detainId, ref DateTime detainDate,
            ref float fineFees, ref int createdByUserId, ref bool isReleased, ref DateTime releaseDate,
            ref int releasedByUserId, ref int releaseApplicationId)
        {
            const string query = "SELECT TOP 1 * FROM DetainedLicenses WHERE LicenseID = @LicenseID " +
                "ORDER BY DetainID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            detainId = (int)reader["DetainID"];
                            detainDate = (DateTime)reader["DetainDate"];
                            fineFees = Convert.ToSingle(reader["FineFees"]);
                            createdByUserId = (int)reader["CreatedByUserID"];
                            isReleased = (bool)reader["IsReleased"];

                            releaseDate = reader["ReleaseDate"] == DBNull.Value
                                ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"];

                            releasedByUserId = reader["ReleasedByUserID"] == DBNull.Value
                                ? -1 : (int)reader["ReleasedByUserID"];

                            releaseApplicationId = reader["ReleaseApplicationID"] == DBNull.Value
                                ? -1 : (int)reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
        {
            const string query = "SELECT * FROM DetainedLicenses_View ORDER BY IsReleased, DetainID";
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

        public static int AddNewDetainedLicense(int licenseId, DateTime detainDate, float fineFees,
            int createdByUserId)
        {
            const string query = @"INSERT INTO dbo.DetainedLicenses
                               (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                               VALUES (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
                               SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);
                    command.Parameters.AddWithValue("@DetainDate", detainDate);
                    command.Parameters.AddWithValue("@FineFees", fineFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);

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

        public static bool UpdateDetainedLicense(int detainId, int licenseId, DateTime detainDate, float fineFees,
            int createdByUserId)
        {
            const string query = @"UPDATE dbo.DetainedLicenses
                              SET LicenseID = @LicenseID, 
                              DetainDate = @DetainDate, 
                              FineFees = @FineFees,
                              CreatedByUserID = @CreatedByUserID
                              WHERE DetainID = @DetainID;";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", detainId);
                    command.Parameters.AddWithValue("@LicenseID", licenseId);
                    command.Parameters.AddWithValue("@DetainDate", detainDate);
                    command.Parameters.AddWithValue("@FineFees", fineFees);
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

        public static bool ReleaseDetainedLicense(int detainId, int releasedByUserId, int releaseApplicationId)
        {
            const string query = @"UPDATE dbo.DetainedLicenses
                              SET IsReleased = 1, 
                              ReleaseDate = @ReleaseDate, 
                              ReleasedByUserID = @ReleasedByUserID,
                              ReleaseApplicationID = @ReleaseApplicationID
                              WHERE DetainID = @DetainID;";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", detainId);
                    command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserId);
                    command.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationId);
                    command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);

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

        public static bool IsLicenseDetained(int licenseId)
        {
            const string query = @"SELECT IsDetained = 1 FROM DetainedLicenses 
                              WHERE LicenseID = @LicenseID AND IsReleased = 0;";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null;
                }
            }
            catch
            {
                return false;
            }
        }


    }

}
