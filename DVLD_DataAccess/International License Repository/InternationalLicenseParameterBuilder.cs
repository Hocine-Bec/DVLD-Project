using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class InternationalLicenseParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, InternationalLicenseDTO internationalLicenseDTO)
        {
            command.Parameters.AddWithValue("@ApplicationID", internationalLicenseDTO.ApplicationID);
            command.Parameters.AddWithValue("@DriverID", internationalLicenseDTO.DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseDTO.IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", internationalLicenseDTO.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseDTO.ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", internationalLicenseDTO.IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseDTO.CreatedByUserID);
        }

        public static void FillSqlCommandParameters(SqlCommand command, InternationalLicenseDTO internationalLicenseDTO, int internationalLicenseId)
        {
            FillSqlCommandParameters(command, internationalLicenseDTO);
            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseId);
        }
    }

}
