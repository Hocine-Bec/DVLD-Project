using DVLD.DTOs;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class UserParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, UsersDTO user)
        {
            command.Parameters.AddWithValue($"@{nameof(user.PersonID)}", user.PersonID);
            command.Parameters.AddWithValue($"@{nameof(user.Username)}", user.Username);
            command.Parameters.AddWithValue($"@{nameof(user.Password)}", user.Password);
            command.Parameters.AddWithValue($"@{nameof(user.IsActive)}", user.IsActive);
        }
    }
}
