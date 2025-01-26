using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class LicenseClassParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, LicenseClassDTO licenseClass)
        {
            command.Parameters.AddWithValue($"@{nameof(licenseClass.ClassName)}", licenseClass.ClassName);
            command.Parameters.AddWithValue($"@{nameof(licenseClass.ClassDescription)}", licenseClass.ClassDescription);
            command.Parameters.AddWithValue($"@{nameof(licenseClass.MinimumAllowedAge)}", licenseClass.MinimumAllowedAge);
            command.Parameters.AddWithValue($"@{nameof(licenseClass.DefaultValidityLength)}", licenseClass.DefaultValidityLength);
            command.Parameters.AddWithValue($"@{nameof(licenseClass.ClassFees)}", licenseClass.ClassFees);
        }
    }
}
