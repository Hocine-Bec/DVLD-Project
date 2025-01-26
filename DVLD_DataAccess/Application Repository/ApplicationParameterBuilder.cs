using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class ApplicationParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, ApplicationDTO applicationDTO)
        {
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.ApplicantPersonID)}", applicationDTO.ApplicantPersonID);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.ApplicationDate)}", applicationDTO.ApplicationDate);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.ApplicationTypeID)}", applicationDTO.ApplicationTypeID);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.ApplicationStatus)}", applicationDTO.ApplicationStatus);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.LastStatusDate)}", applicationDTO.LastStatusDate);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.PaidFees)}", applicationDTO.PaidFees);
            command.Parameters.AddWithValue($"@{nameof(applicationDTO.CreatedByUserID)}", applicationDTO.CreatedByUserID);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int personId, int applicationTypeId, int licenseClassId)
        {
            command.Parameters.AddWithValue($"@{nameof(personId)}", personId);
            command.Parameters.AddWithValue($"@{nameof(applicationTypeId)}", applicationTypeId);
            command.Parameters.AddWithValue($"@{nameof(licenseClassId)}", licenseClassId);
        }
    }


}
