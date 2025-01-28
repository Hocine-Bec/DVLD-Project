using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class AppDataMapper
    {
        public static AppDTO MapToApplicationDTO(SqlDataReader reader)
        {
            return new AppDTO
            {
                AppId = (int)reader["ApplicationID"],
                ApplicantPersonId = (int)reader["ApplicantPersonID"],
                AppDate = (DateTime)reader["ApplicationDate"],
                AppTypeId = (int)reader["ApplicationTypeID"],
                StatusId = (byte)reader["ApplicationStatus"],
                LastStatusDate = (DateTime)reader["LastStatusDate"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                UserID = (int)reader["CreatedByUserID"],
            };
        }
    }


}
