using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class DetainedDataMapper
    {
        public static DetainedDTO MapToDetainedLicenseDTO(SqlDataReader reader)
        {
            return new DetainedDTO
            {
                DetainId = (int)reader["DetainID"],
                LicenseId = (int)reader["LicenseID"],
                DetainDate = (DateTime)reader["DetainDate"],
                FineFees = Convert.ToSingle(reader["FineFees"]),
                CreatedByUserId = (int)reader["CreatedByUserID"],
                IsReleased = (bool)reader["IsReleased"],
                ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"],
                ReleasedByUserId = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"],
                ReleaseAppId = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"]
            };
        }
    }


}
