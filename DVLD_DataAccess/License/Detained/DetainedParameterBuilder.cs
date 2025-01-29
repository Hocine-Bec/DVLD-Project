using DVLD.DTOs;
using System;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class DetainedParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, DetainedDTO dto)
        {
            command.Parameters.AddWithValue("@LicenseID", dto.LicenseId);
            command.Parameters.AddWithValue("@DetainDate", dto.DetainDate);
            command.Parameters.AddWithValue("@FineFees", dto.FineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", dto.CreatedByUserId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int detainId, int releasedByUserId, int releaseApplicationId)
        {
            command.Parameters.AddWithValue("@DetainID", detainId);
            command.Parameters.AddWithValue("@ReleasedByUserID", releasedByUserId);
            command.Parameters.AddWithValue("@ReleaseApplicationID", releaseApplicationId);
            command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
        }
    }


}
