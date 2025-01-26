using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class DriverDataMapper
    {
        public static DriverDTO MapToDriverInfo(SqlDataReader reader)
        {
            return new DriverDTO()
            {
                DriverID = (int)reader["DriverID"],
                PersonID = (int)reader["PersonID"],
                CreatedByUserID = (int)reader["CreatedByUserID"],
                CreatedDate = (DateTime)reader["CreatedDate"]
            };
        }
    }

    

}
