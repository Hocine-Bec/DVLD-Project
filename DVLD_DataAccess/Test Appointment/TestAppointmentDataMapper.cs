using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestAppointmentDataMapper
    {
        public static TestsAppointmentDTO MapToTestAppointmentDTO(SqlDataReader reader)
        {
            return new TestsAppointmentDTO
            {
                TestAppointmentId = (int)reader["TestAppointmentID"],
                TestTypeId = (int)reader["TestTypeID"],
                LocalLicenseAppId = (int)reader["LocalDrivingLicenseApplicationID"],
                AppointmentDate = (DateTime)reader["AppointmentDate"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                UserId = (int)reader["CreatedByUserID"],
                IsLocked = (bool)reader["IsLocked"],
                RetakeTestAppId = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"],
                TestId = (int)reader["TestID"]
            };
        }
    }
}
