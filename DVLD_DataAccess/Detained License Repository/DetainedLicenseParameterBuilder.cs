using System;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class DetainedLicenseParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, int licenseId, DateTime detainDate, float fineFees, int createdByUserId)
        {
            command.Parameters.AddWithValue("@LicenseID", licenseId);
            command.Parameters.AddWithValue("@DetainDate", detainDate);
            command.Parameters.AddWithValue("@FineFees", fineFees);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
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
