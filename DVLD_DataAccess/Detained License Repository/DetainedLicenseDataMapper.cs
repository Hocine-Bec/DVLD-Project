using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class DetainedLicenseDataMapper
    {
        public static DetainedLicensesDTO MapToDetainedLicenseDTO(SqlDataReader reader)
        {
            return new DetainedLicensesDTO
            {
                DetainID = (int)reader["DetainID"],
                LicenseID = (int)reader["LicenseID"],
                DetainDate = (DateTime)reader["DetainDate"],
                FineFees = Convert.ToSingle(reader["FineFees"]),
                CreatedByUserID = (int)reader["CreatedByUserID"],
                IsReleased = (bool)reader["IsReleased"],
                ReleaseDate = reader["ReleaseDate"] == DBNull.Value ? DateTime.MaxValue : (DateTime)reader["ReleaseDate"],
                ReleasedByUserID = reader["ReleasedByUserID"] == DBNull.Value ? -1 : (int)reader["ReleasedByUserID"],
                ReleaseApplicationID = reader["ReleaseApplicationID"] == DBNull.Value ? -1 : (int)reader["ReleaseApplicationID"]
            };
        }
    }


}
