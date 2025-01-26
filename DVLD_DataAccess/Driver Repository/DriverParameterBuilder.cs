using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class DriverParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, int personId, int createdByUserId)
        {
            command.Parameters.AddWithValue("@PersonID", personId);
            command.Parameters.AddWithValue("@CreatedByUserID", createdByUserId);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
        }

        public static void FillSqlCommandParameters(SqlCommand command, DriverDTO driverDTO)
        {
            command.Parameters.AddWithValue($"@{nameof(driverDTO.DriverID)}", driverDTO.DriverID);
            command.Parameters.AddWithValue($"@{nameof(driverDTO.PersonID)}", driverDTO.PersonID);
            command.Parameters.AddWithValue($"@{nameof(driverDTO.CreatedByUserID)}", driverDTO.CreatedByUserID);
        }
    }

    

}
