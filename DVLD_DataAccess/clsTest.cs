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
    public class clsTestData
    {
        public static bool GetTestInfoById(int testId, ref int testAppointmentId, ref bool testResult,
                   ref string notes, ref int createdByUserId)
        {
            const string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            testAppointmentId = (int)reader["TestAppointmentID"];
                            testResult = (bool)reader["TestResult"];
                            notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
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

        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass(int personId, int licenseClassId,
            int testTypeId, ref int testId, ref int testAppointmentId, ref bool testResult,
            ref string notes, ref int createdByUserId)
        {
            const string query = @"
            SELECT TOP 1 
                Tests.TestID, Tests.TestAppointmentID, Tests.TestResult, 
                Tests.Notes, Tests.CreatedByUserID, Applications.ApplicantPersonID
            FROM LocalDrivingLicenseApplications 
            INNER JOIN Tests ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
            INNER JOIN TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID
            INNER JOIN Applications ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
            WHERE Applications.ApplicantPersonID = @PersonID 
                AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID
                AND TestAppointments.TestTypeID = @TestTypeID
            ORDER BY Tests.TestAppointmentID DESC";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
                    command.Parameters.AddWithValue("@TestTypeID", testTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            testId = (int)reader["TestID"];
                            testAppointmentId = (int)reader["TestAppointmentID"];
                            testResult = (bool)reader["TestResult"];
                            notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"];
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

        public static DataTable GetAllTests()
        {
            const string query = "SELECT * FROM Tests ORDER BY TestID";
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

        public static int AddNewTest(int testAppointmentId, bool testResult, string notes, int createdByUserId)
        {
            const string query = @"
            INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID)
            VALUES (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

            UPDATE TestAppointments 
            SET IsLocked = 1 
            WHERE TestAppointmentID = @TestAppointmentID;

            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    command.Parameters.AddWithValue("@TestResult", testResult);
                    command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(notes) ? DBNull.Value : (object)notes);
                    command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);

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

        public static bool UpdateTest(int testId, int testAppointmentId, bool testResult, string notes, int createdByUserId)
        {
            const string query = @"
            UPDATE Tests  
            SET 
                TestAppointmentID = @TestAppointmentID,
                TestResult = @TestResult,
                Notes = @Notes,
                CreatedByUserID = @CreatedByUserID
            WHERE TestID = @TestID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testId);
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
                    command.Parameters.AddWithValue("@TestResult", testResult);
                    command.Parameters.AddWithValue("@Notes", notes);
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

        public static byte GetPassedTestCount(int localDrivingLicenseApplicationId)
        {
            const string query = @"
            SELECT PassedTestCount = COUNT(TestTypeID)
            FROM Tests 
            INNER JOIN TestAppointments ON Tests.TestAppointmentID = TestAppointments.TestAppointmentID
            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                AND TestResult = 1";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && byte.TryParse(result.ToString(), out var passedTestCount)
                        ? passedTestCount : (byte)0;
                }
            }
            catch
            {
                return 0;
            }
        }

    }

}
