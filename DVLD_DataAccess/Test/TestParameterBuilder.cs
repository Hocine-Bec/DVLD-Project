using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, TestsDTO testsDTO)
        {
            command.Parameters.AddWithValue($"@{nameof(testsDTO.TestAppointmentId)}", testsDTO.TestAppointmentId);
            command.Parameters.AddWithValue($"@{nameof(testsDTO.TestResult)}", testsDTO.TestResult);
            command.Parameters.AddWithValue($"@{nameof(testsDTO.Notes)}", string.IsNullOrEmpty(testsDTO.Notes) ? DBNull.Value : (object)testsDTO.Notes);
            command.Parameters.AddWithValue($"@{nameof(testsDTO.UserId)}", testsDTO.UserId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, TestsDTO testsDTO, int testId)
        {
            FillSqlCommandParameters(command, testsDTO);
            command.Parameters.AddWithValue("@TestID", testId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int personId, int licenseClassId, int testTypeId)
        {
            command.Parameters.AddWithValue("@PersonID", personId);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
            command.Parameters.AddWithValue("@TestTypeID", testTypeId);
        }
    }

}
