using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class AppTypeDataMapper
    {
        public static AppTypeDTO MapToApplicationTypeInfo(SqlDataReader reader)
        {
            return new AppTypeDTO()
            {
                ID = (int)reader["ApplicationTypeID"],
                Title = (string)reader["ApplicationTypeTitle"],
                Fees = Convert.ToSingle(reader["ApplicationFees"])
            };
        }
    }

}
