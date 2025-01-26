using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.CountriesRepository;
using System.Net;
using System.Security.Policy;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class TestAppointmentRepository
    {
        public static TestsAppointmentsDTO GetTestAppointmentInfoById(int testAppointmentId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return TestAppointmentDataMapper.MapToTestAppointmentDTO(reader);
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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.GetLastTestAppointment, connection))
                {
                    TestAppointmentParameterBuilder.FillSqlCommandParameters(command, localDrivingLicenseApplicationId, testTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return TestAppointmentDataMapper.MapToTestAppointmentDTO(reader);
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
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.GetAll, connection))
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
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.GetApplicationTestAppointmentsPerTestType, connection))
                {
                    TestAppointmentParameterBuilder.FillSqlCommandParameters(command, localDrivingLicenseApplicationId, testTypeId);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.AddNew, connection))
                {
                    TestAppointmentParameterBuilder.FillSqlCommandParameters(command, testsAppointmentsDTO);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.Update, connection))
                {
                    TestAppointmentParameterBuilder.FillSqlCommandParameters(command, testsAppointmentsDTO, testsAppointmentsDTO.TestAppointmentID);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestAppointmentSqlStatements.GetTestId, connection))
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
