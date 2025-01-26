using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestAppointmentDataMapper
    {
        public static TestsAppointmentsDTO MapToTestAppointmentDTO(SqlDataReader reader)
        {
            return new TestsAppointmentsDTO
            {
                TestAppointmentID = (int)reader["TestAppointmentID"],
                TestTypeID = (int)reader["TestTypeID"],
                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"],
                AppointmentDate = (DateTime)reader["AppointmentDate"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                CreatedByUserID = (int)reader["CreatedByUserID"],
                IsLocked = (bool)reader["IsLocked"],
                RetakeTestApplicationID = reader["RetakeTestApplicationID"] == DBNull.Value ? -1 : (int)reader["RetakeTestApplicationID"]
            };
        }
    }
}
