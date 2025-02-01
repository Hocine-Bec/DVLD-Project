using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestDataMapper
    {
        public static TestsDTO MapToTestDTO(SqlDataReader reader)
        {
            return new TestsDTO
            {
                TestId = (int)reader["TestID"],
                TestAppointmentId = (int)reader["TestAppointmentID"],
                TestResult = (bool)reader["TestResult"],
                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                UserId = (int)reader["CreatedByUserID"]
            };
        }
    }

}
