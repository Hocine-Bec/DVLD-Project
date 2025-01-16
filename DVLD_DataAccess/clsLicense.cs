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
    public class clsLicenseData
    {
        public static bool GetLicenseInfoById(int licenseId, ref int applicationId, ref int driverId, ref int licenseClass,
                  ref DateTime issueDate, ref DateTime expirationDate, ref string notes, ref float paidFees, ref bool isActive,
                  ref byte issueReason, ref int createdByUserId)
        {
            const string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

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
                            applicationId = (int)reader["ApplicationID"];
                            driverId = (int)reader["DriverID"];
                            licenseClass = (int)reader["LicenseClass"];
                            issueDate = (DateTime)reader["IssueDate"];
                            expirationDate = (DateTime)reader["ExpirationDate"];
                            notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
                            paidFees = Convert.ToSingle(reader["PaidFees"]);
                            isActive = (bool)reader["IsActive"];
                            issueReason = (byte)reader["IssueReason"];
                            createdByUserId = (int)reader["DriverID"];
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

        public static DataTable GetAllLicenses()
        {
            const string query = "SELECT * FROM Licenses";
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

        public static DataTable GetDriverLicenses(int driverId)
        {
            const string query = @"
            SELECT     
                Licenses.LicenseID,
                ApplicationID,
                LicenseClasses.ClassName, 
                Licenses.IssueDate, 
                Licenses.ExpirationDate, 
                Licenses.IsActive
            FROM Licenses 
            INNER JOIN LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
            WHERE DriverID = @DriverID
            ORDER BY IsActive DESC, ExpirationDate DESC";

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

        public static int AddNewLicense(int applicationId, int driverId, int licenseClass, DateTime issueDate,
            DateTime expirationDate, string notes, float paidFees, bool isActive, byte issueReason, int createdByUserId)
        {
            const string query = @"
            INSERT INTO Licenses
            (
                ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                Notes, PaidFees, IsActive, IssueReason, CreatedByUserID
            )
            VALUES
            (
                @ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate,
                @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    command.Parameters.AddWithValue("@LicenseClass", licenseClass);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? DBNull.Value : (object)notes);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", issueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var licenseId)
                        ? licenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateLicense(int licenseId, int applicationId, int driverId, int licenseClass,
            DateTime issueDate, DateTime expirationDate, string notes, float paidFees, bool isActive,
            byte issueReason, int createdByUserId)
        {
            const string query = @"
            UPDATE Licenses
            SET 
                ApplicationID = @ApplicationID, 
                DriverID = @DriverID,
                LicenseClass = @LicenseClass,
                IssueDate = @IssueDate,
                ExpirationDate = @ExpirationDate,
                Notes = @Notes,
                PaidFees = @PaidFees,
                IsActive = @IsActive,
                IssueReason = @IssueReason,
                CreatedByUserID = @CreatedByUserID
            WHERE LicenseID = @LicenseID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@DriverID", driverId);
                    command.Parameters.AddWithValue("@LicenseClass", licenseClass);
                    command.Parameters.AddWithValue("@IssueDate", issueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? DBNull.Value : (object)notes);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@IsActive", isActive);
                    command.Parameters.AddWithValue("@IssueReason", issueReason);
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

        public static int GetActiveLicenseIdByPersonId(int personId, int licenseClassId)
        {
            const string query = @"
            SELECT Licenses.LicenseID
            FROM Licenses 
            INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
            WHERE 
                Licenses.LicenseClass = @LicenseClass 
                AND Drivers.PersonID = @PersonID
                AND IsActive = 1;";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@LicenseClass", licenseClassId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var licenseId)
                        ? licenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool DeactivateLicense(int licenseId)
        {
            const string query = @"
            UPDATE Licenses
            SET IsActive = 0
            WHERE LicenseID = @LicenseID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

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
