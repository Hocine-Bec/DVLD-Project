using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class LicenseParameterBuilder
    {

        public static void FillSqlCommandParameters(SqlCommand command, LicenseDTO licenseDTO)
        {
            command.Parameters.AddWithValue("@ApplicationID", licenseDTO.AppId);
            command.Parameters.AddWithValue("@DriverID", licenseDTO.DriverId);
            command.Parameters.AddWithValue("@LicenseClass", licenseDTO.LicenseClassId);
            command.Parameters.AddWithValue("@IssueDate", licenseDTO.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", licenseDTO.ExpirationDate);
            command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(licenseDTO.Notes) ? DBNull.Value : (object)licenseDTO.Notes);
            command.Parameters.AddWithValue("@PaidFees", licenseDTO.PaidFees);
            command.Parameters.AddWithValue("@IsActive", licenseDTO.IsActive);
            command.Parameters.AddWithValue("@IssueReason", licenseDTO.IssueReasonId);
            command.Parameters.AddWithValue("@CreatedByUserID", licenseDTO.UserID);
        }

        public static void FillSqlCommandParameters(SqlCommand command, LicenseDTO licenseDTO, int licenseId)
        {
            FillSqlCommandParameters(command, licenseDTO);
            command.Parameters.AddWithValue("@LicenseID", licenseId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int personId, int licenseClassId)
        {
            command.Parameters.AddWithValue("@PersonID", personId);
            command.Parameters.AddWithValue("@LicenseClass", licenseClassId);
        }
    }
}
