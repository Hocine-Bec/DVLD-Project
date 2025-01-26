using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class ApplicationDataMapper
    {
        public static ApplicationDTO MapToApplicationDTO(SqlDataReader reader)
        {
            return new ApplicationDTO
            {
                ApplicationID = (int)reader["ApplicationID"],
                ApplicantPersonID = (int)reader["ApplicantPersonID"],
                ApplicationDate = (DateTime)reader["ApplicationDate"],
                ApplicationTypeID = (int)reader["ApplicationTypeID"],
                ApplicationStatus = (byte)reader["ApplicationStatus"],
                LastStatusDate = (DateTime)reader["LastStatusDate"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                CreatedByUserID = (int)reader["CreatedByUserID"],
            };
        }
    }


}
