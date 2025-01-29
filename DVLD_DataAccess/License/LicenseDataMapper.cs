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
                LicenseId = (int)reader["LicenseID"],
                AppId = (int)reader["ApplicationID"],
                DriverId = (int)reader["DriverID"],
                LicenseClassId = (int)reader["LicenseClass"],
                IssueDate = (DateTime)reader["IssueDate"],
                ExpirationDate = (DateTime)reader["ExpirationDate"],
                Notes = reader["Notes"] == DBNull.Value ? "" : (string)reader["Notes"],
                PaidFees = Convert.ToSingle(reader["PaidFees"]),
                IsActive = (bool)reader["IsActive"],
                IssueReasonId = (byte)reader["IssueReason"],
                UserID = (int)reader["CreatedByUserID"]
            };
        }
    }
}
