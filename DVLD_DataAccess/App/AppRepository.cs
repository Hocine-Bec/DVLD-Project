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
    public class AppRepository
    {
        public AppDTO GetApplicationInfoById(int appId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", appId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return AppDataMapper.MapToApplicationDTO(reader);
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

        public DataTable GetAllApplications()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.GetAll, connection))
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

        public int AddNewApplication(AppDTO appDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.AddNew, connection))
                {
                    AppParameterBuilder.FillSqlCommandParameters(command, appDTO);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var appId)
                        ? appId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdateApplication(AppDTO appDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.Update, connection))
                {
                    AppParameterBuilder.FillSqlCommandParameters(command, appDTO);
                    command.Parameters.AddWithValue("@ApplicationID", appDTO.AppId);

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

        public bool DeleteApplication(int applicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.Delete, connection))
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

        public bool IsApplicationExist(int applicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.IsExist, connection))
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

        public bool DoesPersonHaveActiveApplication(int personId, int applicationTypeId)
        {
            return GetActiveApplicationId(personId, applicationTypeId) != -1;
        }

        public int GetActiveApplicationId(int personId, int applicationTypeId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.GetActiveApplicationId, connection))
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

        public int GetActiveAppIdForLicenseClass(int personId, int applicationTypeId, int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.GetActiveApplicationIdForLicenseClass, connection))
                {
                    AppParameterBuilder.FillSqlCommandParameters(command, personId, applicationTypeId, licenseClassId);

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

        public bool UpdateStatus(int applicationId, short newStatus)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(AppSqlStatements.UpdateStatus, connection))
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
