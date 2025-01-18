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
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {
        public static TestsAppointmentsDTO GetTestAppointmentInfoById(int testAppointmentId)
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

                            return new TestsAppointmentsDTO()
                            {
                                TestTypeID = (int)reader["TestTypeID"],
                                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
                                AppointmentDate = (DateTime)reader["AppointmentDate"],
                                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                                CreatedByUserID = (int)reader["CreatedByUserID"],
                                IsLocked = (bool)reader["IsLocked"],

                                RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value 
                                          ? -1 : (int)reader["RetakeTestApplicationID"]
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

        public static TestsAppointmentsDTO GetLastTestAppointment(int localDrivingLicenseApplicationId, int testTypeId)
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
                            return new TestsAppointmentsDTO()
                            {
                                TestAppointmentID = (int)reader["TestAppointmentID"],
                                AppointmentDate = (DateTime)reader["AppointmentDate"],
                                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                                CreatedByUserID = (int)reader["CreatedByUserID"],
                                IsLocked = (bool)reader["IsLocked"],
                                RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"]
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

        public static int AddNewTestAppointment(TestsAppointmentsDTO testsAppointmentsDTO)
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
                    command.Parameters.AddWithValue("@TestTypeID", testsAppointmentsDTO.TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testsAppointmentsDTO.LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", testsAppointmentsDTO.AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", testsAppointmentsDTO.PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", testsAppointmentsDTO.CreatedByUserID);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", testsAppointmentsDTO.RetakeTestApplicationID == -1 
                         ? DBNull.Value : (object)testsAppointmentsDTO.RetakeTestApplicationID);

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

        public static bool UpdateTestAppointment(TestsAppointmentsDTO testsAppointmentsDTO)
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
                    command.Parameters.AddWithValue("@TestAppointmentID", testsAppointmentsDTO.TestAppointmentID);
                    command.Parameters.AddWithValue("@TestTypeID", testsAppointmentsDTO.TestTypeID);
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testsAppointmentsDTO.LocalDrivingLicenseApplicationID);
                    command.Parameters.AddWithValue("@AppointmentDate", testsAppointmentsDTO.AppointmentDate);
                    command.Parameters.AddWithValue("@PaidFees", testsAppointmentsDTO.PaidFees);
                    command.Parameters.AddWithValue("@CreatedByUserID", testsAppointmentsDTO.CreatedByUserID);
                    command.Parameters.AddWithValue("@RetakeTestApplicationID", testsAppointmentsDTO.RetakeTestApplicationID == -1 
                        ? DBNull.Value : (object)testsAppointmentsDTO.RetakeTestApplicationID);

                    command.Parameters.AddWithValue("@IsLocked", testsAppointmentsDTO.IsLocked);
                    
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
