using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class LicenseDataMapper
    {
        public static LicenseDTO MapToLicenseDTO(SqlDataReader reader)
        {
            return new LicenseDTO
            {
                LicenseID = (int)reader["LicenseID"],
                ApplicationID = (int)reader["ApplicationID"],
                DriverID = (int)reader["DriverID"],
                LicenseClass = (int)reader["LicenseClass"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                IsActive = (bool)reader["IsActive"],
                IssueReason = (byte)reader["IssueReason"],
                CreatedByUserID = (int)reader["CreatedByUserID"]
            };
        }
    }
}
