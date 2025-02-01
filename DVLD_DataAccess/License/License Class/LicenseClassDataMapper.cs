using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class LicenseClassDataMapper
    {
        public static LicenseClassDTO MapToLicenseClassDTO(SqlDataReader reader)
        {
            return new LicenseClassDTO
            {
                LicenseClassID = (int)reader["LicenseClassID"],
                ClassName = (string)reader["ClassName"],
                ClassDescription = (string)reader["ClassDescription"],
                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
                DefaultValidityLength = (byte)reader["DefaultValidityLength"],
                ClassFees = Convert.ToSingle(reader["ClassFees"]),
            };
        }
    }
}
