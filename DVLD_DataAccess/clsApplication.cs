using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
   
    public class clsApplicationData
    {
        public static ApplicationDTO GetApplicationInfoById(int applicationId)
        {
            const string query = "SELECT * FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ApplicationDTO()
                            {
                                ApplicantPersonID = (int)reader["ApplicantPersonID"],
                                ApplicationDate = (DateTime)reader["ApplicationDate"],
                                ApplicationTypeID = (int)reader["ApplicationTypeID"],
                                ApplicationStatus = (byte)reader["ApplicationStatus"],
                                LastStatusDate = (DateTime)reader["LastStatusDate"],
                                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                                CreatedByUserID = (int)reader["CreatedByUserID"],
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

        public static DataTable GetAllApplications()
        {
            const string query = "SELECT * FROM ApplicationsList_View ORDER BY ApplicationDate";
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

        public static int AddNewApplication(ApplicationDTO applicationDTO)
        {
            const string query = @"
            INSERT INTO Applications 
            (
                ApplicantPersonID, ApplicationDate, ApplicationTypeID,
                ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID
            )
            VALUES 
            (
                @ApplicantPersonID, @ApplicationDate, @ApplicationTypeID,
                @ApplicationStatus, @LastStatusDate, @PaidFees, @CreatedByUserID
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", applicationDTO.ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicationDate",   applicationDTO.ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationDTO.ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationStatus", applicationDTO.ApplicationStatus);
                    command.Parameters.AddWithValue("@LastStatusDate",    applicationDTO.LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees",          applicationDTO.PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID",   applicationDTO.CreatedByUserID);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var applicationId)
                        ? applicationId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateApplication(ApplicationDTO applicationDTO)
        {
            const string query = @"
            UPDATE Applications  
            SET 
                ApplicantPersonID = @ApplicantPersonID,
                ApplicationDate = @ApplicationDate,
                ApplicationTypeID = @ApplicationTypeID,
                ApplicationStatus = @ApplicationStatus, 
                LastStatusDate = @LastStatusDate,
                PaidFees = @PaidFees,
                CreatedByUserID = @CreatedByUserID
            WHERE ApplicationID = @ApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                { 
                    command.Parameters.AddWithValue("@ApplicationID", applicationDTO.ApplicantPersonID);
                    command.Parameters.AddWithValue("@ApplicantPersonID", applicationDTO.ApplicationDate);
                    command.Parameters.AddWithValue("@ApplicationDate", applicationDTO.ApplicationTypeID);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationDTO.ApplicationStatus);
                    command.Parameters.AddWithValue("@ApplicationStatus", applicationDTO.LastStatusDate);
                    command.Parameters.AddWithValue("@LastStatusDate", applicationDTO.LastStatusDate);
                    command.Parameters.AddWithValue("@PaidFees", applicationDTO.PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", applicationDTO.CreatedByUserID);

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

        public static bool DeleteApplication(int applicationId)
        {
            const string query = "DELETE FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

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

        public static bool IsApplicationExist(int applicationId)
        {
            const string query = "SELECT Found = 1 FROM Applications WHERE ApplicationID = @ApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

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

        public static bool DoesPersonHaveActiveApplication(int personId, int applicationTypeId)
        {
            return GetActiveApplicationId(personId, applicationTypeId) != -1;
        }

        public static int GetActiveApplicationId(int personId, int applicationTypeId)
        {
            const string query = @"
            SELECT ApplicationID 
            FROM Applications 
            WHERE ApplicantPersonID = @ApplicantPersonID 
                AND ApplicationTypeID = @ApplicationTypeID 
                AND ApplicationStatus = 1";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", personId);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var activeApplicationId)
                        ? activeApplicationId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static int GetActiveApplicationIdForLicenseClass(int personId, int applicationTypeId, int licenseClassId)
        {
            const string query = @"
            SELECT Applications.ApplicationID  
            FROM Applications 
            INNER JOIN LocalDrivingLicenseApplications 
                ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
            WHERE ApplicantPersonID = @ApplicantPersonID 
                AND ApplicationTypeID = @ApplicationTypeID 
                AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                AND ApplicationStatus = 1";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicantPersonID", personId);
                    command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var activeApplicationId)
                        ? activeApplicationId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateStatus(int applicationId, short newStatus)
        {
            const string query = @"
            UPDATE Applications  
            SET 
                ApplicationStatus = @NewStatus, 
                LastStatusDate = @LastStatusDate
            WHERE ApplicationID = @ApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@NewStatus", newStatus);
                    command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);

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
