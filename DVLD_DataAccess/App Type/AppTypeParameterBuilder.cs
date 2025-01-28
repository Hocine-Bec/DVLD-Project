using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class AppTypeParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, AppTypeDTO appTypeDTO)
        {
            command.Parameters.AddWithValue("@ApplicationTypeID", appTypeDTO.ID);
            command.Parameters.AddWithValue("@Title", appTypeDTO.Title);
            command.Parameters.AddWithValue("@Fees", appTypeDTO.Fees);
        }

        public static void FillSqlCommandParameters(SqlCommand command, int applicationTypeId)
        {
            command.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeId);
        }
    }

}
