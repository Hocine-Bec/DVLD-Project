using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class LocalLicenseAppParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, int localDrivingLicenseApplicationId, int applicationId, int licenseClassId)
        {
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
            command.Parameters.AddWithValue("@ApplicationID", applicationId);
            command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int localDrivingLicenseApplicationId, int testTypeId)
        {
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", localDrivingLicenseApplicationId);
            command.Parameters.AddWithValue("@TestTypeID", testTypeId);
        }
    }


}



