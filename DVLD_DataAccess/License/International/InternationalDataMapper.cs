using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class InternationalDataMapper
    {
        public static InternationalDTO MapToInternationalLicenseDTO(SqlDataReader reader)
        {
            return new InternationalDTO
            {
                InternationalId = (int)reader["InternationalLicenseID"],
                AppId = (int)reader["ApplicationID"],
                DriverId = (int)reader["DriverID"],
                IssuedUsingLicenseId = (int)reader["IssuedUsingLocalLicenseID"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpireDate = (DateTime)reader["ExpirationDate"],
                IsActive = (bool)reader["IsActive"],
                UserId = (int)reader["CreatedByUserID"]
            };
        }
    }

}
