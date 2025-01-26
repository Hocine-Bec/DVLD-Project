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
    public class ApplicationRepository
    {
        public static ApplicationDTO GetApplicationInfoById(int applicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ApplicationDataMapper.MapToApplicationDTO(reader);
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
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.GetAll, connection))
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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.AddNew, connection))
                {
                    ApplicationParameterBuilder.FillSqlCommandParameters(command, applicationDTO);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.Update, connection))
                {
                    ApplicationParameterBuilder.FillSqlCommandParameters(command, applicationDTO);
                    command.Parameters.AddWithValue("@ApplicationID", applicationDTO.ApplicationID);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.Delete, connection))
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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.IsExist, connection))
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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.GetActiveApplicationId, connection))
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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.GetActiveApplicationIdForLicenseClass, connection))
                {
                    ApplicationParameterBuilder.FillSqlCommandParameters(command, personId, applicationTypeId, licenseClassId);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(ApplicationSqlStatements.UpdateStatus, connection))
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
