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
    public class TestRepository
    {
        public static TestsDTO GetTestInfoById(int testId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@TestID", testId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return TestDataMapper.MapToTestDTO(reader);
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

        public static TestsDTO GetLastTestByPersonAndTestTypeAndLicenseClass(int personId, int licenseClassId, int testTypeId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.GetLastTestByPersonAndTestTypeAndLicenseClass, connection))
                {
                    TestParameterBuilder.FillSqlCommandParameters(command, personId, licenseClassId, testTypeId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return TestDataMapper.MapToTestDTO(reader);
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

        public static DataTable GetAllTests()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.GetAll, connection))
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

        public static int AddNewTest(TestsDTO testsDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.AddNew, connection))
                {
                    TestParameterBuilder.FillSqlCommandParameters(command, testsDTO);

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

        public static bool UpdateTest(TestsDTO testsDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.Update, connection))
                {
                    TestParameterBuilder.FillSqlCommandParameters(command, testsDTO, testsDTO.TestID);

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
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(TestSqlStatements.GetPassedTestCount, connection))
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
