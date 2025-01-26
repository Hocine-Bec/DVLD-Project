using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class LocalLicenseAppDataMapper
    {
        public static (int ApplicationID, int LicenseClassID) MapToLocalLicenseAppInfo(SqlDataReader reader)
        {
            return (
                ApplicationID: (int)reader["ApplicationID"],
                LicenseClassID: (int)reader["LicenseClassID"]
            );
        }
    }


}



