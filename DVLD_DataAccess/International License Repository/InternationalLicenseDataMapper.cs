using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class InternationalLicenseDataMapper
    {
        public static InternationalLicenseDTO MapToInternationalLicenseDTO(SqlDataReader reader)
        {
            return new InternationalLicenseDTO
            {
                InternationalLicenseID = (int)reader["InternationalLicenseID"],
                ApplicationID = (int)reader["ApplicationID"],
                DriverID = (int)reader["DriverID"],
                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                IsActive = (bool)reader["IsActive"],
                CreatedByUserID = (int)reader["CreatedByUserID"]
            };
        }
    }

}
