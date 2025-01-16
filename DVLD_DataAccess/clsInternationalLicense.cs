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
using System.ComponentModel;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoById(int internationalLicenseId, ref int applicationId,
                    ref int driverId, ref int issuedUsingLocalLicenseId, ref DateTime issueDate, ref DateTime expirationDate,
                    ref bool isActive, ref int createdByUserId)
        {
            const string query = "SELECT * FROM InternationalLicenses " +
                "WHERE InternationalLicenseID = @InternationalLicenseID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            applicationId = (int)reader["ApplicationID"];
                            driverId = (int)reader["DriverID"];
                            issuedUsingLocalLicenseId = (int)reader["IssuedUsingLocalLicenseID"];
                            issueDate = (DateTime)reader["IssueDate"];
                            expirationDate = (DateTime)reader["ExpirationDate"];
                            isActive = (bool)reader["IsActive"];
                            createdByUserId = (int)reader["CreatedByUserID"];
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

        public static DataTable GetAllInternationalLicenses()
        {
            const string query = @"
            SELECT InternationalLicenseID, ApplicationID, DriverID,
                   IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive
            FROM InternationalLicenses
            ORDER BY IsActive, ExpirationDate DESC";

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

        public static DataTable GetDriverInternationalLicenses(int driverId)
        {
            const string query = "SELECT InternationalLicenseID, ApplicationID, IssuedUsingLocalLicenseID, " +
                "IssueDate, ExpirationDate, IsActive " +
                "FROM InternationalLicenses " +
                "WHERE DriverID = @DriverID ORDER BY ExpirationDate DESC";

            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);

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

        public static int AddNewInternationalLicense(int applicationId, int driverId,
            int issuedUsingLocalLicenseId, DateTime issueDate, DateTime expirationDate,
            bool isActive, int createdByUserId)
        {
            const string query = @"
            UPDATE InternationalLicenses
            SET IsActive = 0
            WHERE DriverID = @DriverID;

            INSERT INTO InternationalLicenses
            (
                ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                IssueDate, ExpirationDate, IsActive, CreatedByUserID
            )
            VALUES
            (
                @ApplicationID, @DriverID, @IssuedUsingLocalLicenseID,
                @IssueDate, @ExpirationDate, @IsActive, @CreatedByUserID
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseId);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@IsActive", isActive);
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

        public static bool UpdateInternationalLicense(int internationalLicenseId, int applicationId,
            int driverId, int issuedUsingLocalLicenseId, DateTime issueDate, DateTime expirationDate,
            bool isActive, int createdByUserId)
        {
            const string query = @"
            UPDATE InternationalLicenses
            SET 
                ApplicationID = @ApplicationID,
                DriverID = @DriverID,
                IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                IssueDate = @IssueDate,
                ExpirationDate = @ExpirationDate,
                IsActive = @IsActive,
                CreatedByUserID = @CreatedByUserID
            WHERE InternationalLicenseID = @InternationalLicenseID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseId);
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", issuedUsingLocalLicenseId);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@IsActive", isActive);
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

        public static int GetActiveInternationalLicenseIdByDriverId(int driverId)
        {
            const string query = @"
            SELECT TOP 1 InternationalLicenseID
            FROM InternationalLicenses
            WHERE DriverID = @DriverID AND GETDATE() BETWEEN IssueDate AND ExpirationDate
            ORDER BY ExpirationDate DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var internationalLicenseId)
                       ? internationalLicenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

    }

}
