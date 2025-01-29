using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class AppParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, AppDTO appDTO)
        {
            command.Parameters.AddWithValue($"@{nameof(appDTO.PersonId)}", appDTO.PersonId);
            command.Parameters.AddWithValue($"@{nameof(appDTO.AppDate)}", appDTO.AppDate);
            command.Parameters.AddWithValue($"@{nameof(appDTO.AppTypeId)}", appDTO.AppTypeId);
            command.Parameters.AddWithValue($"@{nameof(appDTO.StatusId)}", appDTO.StatusId);
            command.Parameters.AddWithValue($"@{nameof(appDTO.LastStatusDate)}", appDTO.LastStatusDate);
            command.Parameters.AddWithValue($"@{nameof(appDTO.PaidFees)}", appDTO.PaidFees);
            command.Parameters.AddWithValue($"@{nameof(appDTO.UserID)}", appDTO.UserID);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int personId, int applicationTypeId, int licenseClassId)
        {
            command.Parameters.AddWithValue($"@{nameof(personId)}", personId);
            command.Parameters.AddWithValue($"@{nameof(applicationTypeId)}", applicationTypeId);
            command.Parameters.AddWithValue($"@{nameof(licenseClassId)}", licenseClassId);
        }
    }


}
