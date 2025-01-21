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
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {
        public static LicenseDTO GetLicenseInfoById(int licenseId)
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
                            return new LicenseDTO()
                            {
                                ApplicationID = (int)reader["ApplicationID"],
                                DriverID = (int)reader["DriverID"],
                                LicenseClass = (int)reader["LicenseClass"],
                                IssueDate = (DateTime)reader["IssueDate"],
                                ExpirationDate = (DateTime)reader["ExpirationDate"],
                                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                                IsActive = (bool)reader["IsActive"],
                                IssueReason = (byte)reader["IssueReason"],
                                CreatedByUserID = (int)reader["DriverID"]
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

        public static int AddNewLicense(LicenseDTO licenseDTO)
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
                    command.Parameters.AddWithValue("@ApplicationID", licenseDTO.ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", licenseDTO.DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", licenseDTO.LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", licenseDTO.IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", licenseDTO.ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(licenseDTO.Notes) ? DBNull.Value : (object)licenseDTO.Notes);
                    command.Parameters.AddWithValue("@PaidFees", licenseDTO.PaidFees);
                    command.Parameters.AddWithValue("@IsActive", licenseDTO.IsActive);
                    command.Parameters.AddWithValue("@IssueReason", licenseDTO.IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", licenseDTO.CreatedByUserID);

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

        public static bool UpdateLicense(LicenseDTO licenseDTO)
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
                    command.Parameters.AddWithValue("@LicenseID", licenseDTO.LicenseID);
                    command.Parameters.AddWithValue("@ApplicationID", licenseDTO.ApplicationID);
                    command.Parameters.AddWithValue("@DriverID", licenseDTO.DriverID);
                    command.Parameters.AddWithValue("@LicenseClass", licenseDTO.LicenseClass);
                    command.Parameters.AddWithValue("@IssueDate", licenseDTO.IssueDate);
                    command.Parameters.AddWithValue("@ExpirationDate", licenseDTO.ExpirationDate);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(licenseDTO.Notes) ? DBNull.Value : (object)licenseDTO.Notes);
                    command.Parameters.AddWithValue("@PaidFees", licenseDTO.PaidFees);
                    command.Parameters.AddWithValue("@IsActive", licenseDTO.IsActive);
                    command.Parameters.AddWithValue("@IssueReason", licenseDTO.IssueReason);
                    command.Parameters.AddWithValue("@CreatedByUserID", licenseDTO.CreatedByUserID);

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
