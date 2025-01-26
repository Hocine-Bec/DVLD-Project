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
                TestID = (int)reader["TestID"],
                TestAppointmentID = (int)reader["TestAppointmentID"],
                TestResult = (bool)reader["TestResult"],
                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                CreatedByUserID = (int)reader["CreatedByUserID"]
            };
        }
    }

}
