using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class InternationalParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, InternationalDTO internationalLicenseDTO)
        {
            command.Parameters.AddWithValue("@ApplicationID", internationalLicenseDTO.AppId);
            command.Parameters.AddWithValue("@DriverID", internationalLicenseDTO.DriverId);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", internationalLicenseDTO.IssuedUsingLicenseId);
            command.Parameters.AddWithValue("@IssueDate", internationalLicenseDTO.IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", internationalLicenseDTO.ExpireDate);
            command.Parameters.AddWithValue("@IsActive", internationalLicenseDTO.IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", internationalLicenseDTO.UserId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, InternationalDTO internationalLicenseDTO, int internationalLicenseId)
        {
            FillSqlCommandParameters(command, internationalLicenseDTO);
            command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseId);
        }
    }

}
