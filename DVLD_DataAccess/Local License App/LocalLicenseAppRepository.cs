using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices.WindowsRuntime;

namespace DVLD_DataAccess
{
    public class LocalLicenseAppRepository
    {
        //Local License
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int localDrivingLicenseApplicationID, ref int applicationID, ref int licenseClassID)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.GetByID, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var result = LocalLicenseAppDataMapper.MapToLocalLicenseAppInfo(reader);
                            applicationID = result.ApplicationID;
                            licenseClassID = result.LicenseClassID;
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

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationId(int applicationId, ref int localDrivingLicenseApplicationId, ref int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.GetByApplicationId, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var result = LocalLicenseAppDataMapper.MapToLocalLicenseAppInfo(reader);
                            localDrivingLicenseApplicationId = (int)reader["LocalDrivingLicenseApplicationID"];
                            licenseClassId = result.LicenseClassID;
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

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.GetAll, connection))
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

        public static int AddNewLocalDrivingLicenseApplication(int applicationId, int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.AddNew, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

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

        public static bool UpdateLocalDrivingLicenseApplication(int localDrivingLicenseApplicationId, int applicationId, int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.Update, connection))
                {
                    LocalLicenseAppParameterBuilder.FillSqlCommandParameters(command, localDrivingLicenseApplicationId, applicationId, licenseClassId);

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

        public static bool DeleteLocalDrivingLicenseApplication(int localDrivingLicenseApplicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LocalLicenseAppSqlStatements.Delete, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);

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



