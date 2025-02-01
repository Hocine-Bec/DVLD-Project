using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestAppointmentParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, int localDrivingLicenseApplicationId, int testTypeId)
        {
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
            command.Parameters.AddWithValue("@TestTypeID", testTypeId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, TestsAppointmentDTO testsAppointmentsDTO)
        {
            command.Parameters.AddWithValue("@TestTypeID", testsAppointmentsDTO.TestTypeId);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", testsAppointmentsDTO.LocalLicenseAppId);
            command.Parameters.AddWithValue("@AppointmentDate", testsAppointmentsDTO.AppointmentDate);
            command.Parameters.AddWithValue("@PaidFees", testsAppointmentsDTO.PaidFees);
            command.Parameters.AddWithValue("@CreatedByUserID", testsAppointmentsDTO.UserId);
            command.Parameters.AddWithValue("@RetakeTestApplicationID", testsAppointmentsDTO.RetakeTestAppId == -1
                ? DBNull.Value : (object)testsAppointmentsDTO.RetakeTestAppId);
            command.Parameters.AddWithValue("@IsLocked", testsAppointmentsDTO.IsLocked);
        }

        public static void FillSqlCommandParameters(SqlCommand command, TestsAppointmentDTO testsAppointmentsDTO, int testAppointmentId)
        {
            FillSqlCommandParameters(command, testsAppointmentsDTO);
            command.Parameters.AddWithValue("@TestAppointmentID", testAppointmentId);
        }
    }
}
