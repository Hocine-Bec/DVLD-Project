using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class CountryParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, int countryId)
        {
            command.Parameters.AddWithValue("@CountryID", countryId);
        }

        public static void FillSqlCommandParameters(SqlCommand command, string countryName)
        {
            command.Parameters.AddWithValue("@CountryName", countryName);
        }
    }
   

}
