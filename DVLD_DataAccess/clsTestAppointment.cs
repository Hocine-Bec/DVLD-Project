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
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static bool GetTestAppointmentInfoById(int testAppointmentId, ref int testTypeId,
                   ref int localDrivingLicenseApplicationId, ref DateTime appointmentDate, ref float paidFees,
                   ref int createdByUserId, ref bool isLocked, ref int retakeTestApplicationId)
        {
            const string query = "SELECT * FROM TestAppointments WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            testTypeId = (int)reader["TestTypeID"];
                            localDrivingLicenseApplicationId = (int)reader["LocalDrivingLicenseApplicationID"];
                            appointmentDate = (DateTime)reader["AppointmentDate"];
                            paidFees = Convert.ToSingle(reader["PaidFees"]);
                            createdByUserId = (int)reader["CreatedByUserID"];
                            isLocked = (bool)reader["IsLocked"];
                            retakeTestApplicationId = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"];

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

        public static bool GetLastTestAppointment(int localDrivingLicenseApplicationId, int testTypeId,
            ref int testAppointmentId, ref DateTime appointmentDate, ref float paidFees,
            ref int createdByUserId, ref bool isLocked, ref int retakeTestApplicationId)
        {
            const string query = @"
            SELECT TOP 1 *
            FROM TestAppointments
            WHERE TestTypeID = @TestTypeID 
                AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
            ORDER BY TestAppointmentID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            testAppointmentId = (int)reader["TestAppointmentID"];
                            appointmentDate = (DateTime)reader["AppointmentDate"];
                            paidFees = Convert.ToSingle(reader["PaidFees"]);
                            createdByUserId = (int)reader["CreatedByUserID"];
                            isLocked = (bool)reader["IsLocked"];
                            retakeTestApplicationId = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"];
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

        public static DataTable GetAllTestAppointments()
        {
            const string query = "SELECT * FROM TestAppointments_View ORDER BY AppointmentDate DESC";
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

        public static DataTable GetApplicationTestAppointmentsPerTestType(int localDrivingLicenseApplicationId, int testTypeId)
        {
            const string query = @"
            SELECT TestAppointmentID, AppointmentDate, PaidFees, IsLocked
            FROM TestAppointments
            WHERE TestTypeID = @TestTypeID 
                AND LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
            ORDER BY TestAppointmentID DESC";

            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

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

        public static int AddNewTestAppointment(int testTypeId, int localDrivingLicenseApplicationId,
            DateTime appointmentDate, float paidFees, int createdByUserId, int retakeTestApplicationId)
        {
            const string query = @"
            INSERT INTO TestAppointments 
            (
                TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, 
                PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID
            )
            VALUES 
            (
                @TestTypeID, @LocalDrivingLicenseApplicationID, @AppointmentDate, 
                @PaidFees, @CreatedByUserID, 0, @RetakeTestApplicationID
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationId == -1 ? DBNull.Value : (object)retakeTestApplicationId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var testAppointmentId)
                        ? testAppointmentId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateTestAppointment(int testAppointmentId, int testTypeId, int localDrivingLicenseApplicationId,
            DateTime appointmentDate, float paidFees, int createdByUserId, bool isLocked, int retakeTestApplicationId)
        {
            const string query = @"
            UPDATE TestAppointments  
            SET 
                TestTypeID = @TestTypeID,
                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID,
                AppointmentDate = @AppointmentDate,
                PaidFees = @PaidFees,
                CreatedByUserID = @CreatedByUserID,
                IsLocked = @IsLocked,
                RetakeTestApplicationID = @RetakeTestApplicationID
            WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", paidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
                    command.Parameters.AddWithValue("@IsLocked", isLocked);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", retakeTestApplicationId == -1 ? DBNull.Value : (object)retakeTestApplicationId);

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

        public static int GetTestId(int testAppointmentId)
        {
            const string query = "SELECT TestID FROM Tests WHERE TestAppointmentID = @TestAppointmentID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var testId)
                        ? testId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }
    }

}
