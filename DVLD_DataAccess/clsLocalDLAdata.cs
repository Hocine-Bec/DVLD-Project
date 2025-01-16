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
    public class clsLocalDLAdata
    {
        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID, 
            ref int ApplicationID, ref int LicenseClassID)
        {
            const string query = @"SELECT * FROM LocalDrivingLicenseApplications 
                          WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ApplicationID = (int)reader["ApplicationID"];
                            LicenseClassID = (int)reader["LicenseClassID"];
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

        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationId(int applicationId, 
            ref int localDrivingLicenseApplicationId, ref int licenseClassId)
        {
            const string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

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
                            localDrivingLicenseApplicationId = (int)reader["LocalDrivingLicenseApplicationID"];
                            licenseClassId = (int)reader["LicenseClassID"];
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
            const string query = @"SELECT * FROM LocalDrivingLicenseApplications_View ORDER BY ApplicationDate DESC";
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

        public static int AddNewLocalDrivingLicenseApplication(int applicationId, int licenseClassId)
        {
            const string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)
                             VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var insertedId)
                    ? insertedId : -1 ;
                    
                }
            }
            catch
            {
                return -1;
            }
        }


        public static bool UpdateLocalDrivingLicenseApplication(
         int localDrivingLicenseApplicationId, int applicationId, int licenseClassId)
        {
            const string query = @"UPDATE LocalDrivingLicenseApplications  
                            SET ApplicationID = @ApplicationID,
                                LicenseClassID = @LicenseClassID
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

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
            const string query = @"DELETE FROM LocalDrivingLicenseApplications 
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
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


        public static bool DoesPassTestType(int localDrivingLicenseApplicationId, int testTypeId)
        {
            const string query = @"SELECT TOP 1 TestResult
                            FROM LocalDrivingLicenseApplications 
                            INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                            AND TestAppointments.TestTypeID = @TestTypeID
                            ORDER BY TestAppointments.TestAppointmentID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && bool.TryParse(result.ToString(), out var returnedResult)
                    ? returnedResult : false;
                    
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool DoesAttendTestType(int localDrivingLicenseApplicationId, int testTypeId)
        {
            const string query = @"SELECT TOP 1 Found = 1
                            FROM LocalDrivingLicenseApplications 
                            INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                            AND TestAppointments.TestTypeID = @TestTypeID
                            ORDER BY TestAppointments.TestAppointmentID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

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

        public static byte TotalTrialsPerTest(int localDrivingLicenseApplicationId, int testTypeId)
        {
            const string query = @"SELECT TotalTrialsPerTest = COUNT(TestID)
                            FROM LocalDrivingLicenseApplications 
                            INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            INNER JOIN Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                            AND TestAppointments.TestTypeID = @TestTypeID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    if (result != null && byte.TryParse(result.ToString(), out var trials))
                    {
                        return trials;
                    }

                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static bool IsThereAnActiveScheduledTest(int localDrivingLicenseApplicationId, int testTypeId)
        {
            const string query = @"SELECT TOP 1 Found = 1
                            FROM LocalDrivingLicenseApplications 
                            INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID 
                            WHERE LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID  
                            AND TestAppointments.TestTypeID = @TestTypeID 
                            AND IsLocked = 0
                            ORDER BY TestAppointments.TestAppointmentID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

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



