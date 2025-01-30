using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class CountryDataMapper
    {
        public static CountriesDTO MapToCountriesDTO(SqlDataReader reader)
        {
            return new CountriesDTO()
            {
                Id = (int)reader["CountryID"],
                CountryName = (string)reader["CountryName"]
            };

        }
    }
   

}
