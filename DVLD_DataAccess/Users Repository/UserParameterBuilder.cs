using DVLD.DTOs;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class UserParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, UsersDTO user)
        {
            command.Parameters.AddWithValue("@PersonID", user.PersonID);
            command.Parameters.AddWithValue("@UserName", user.Username);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
        }
    }
}
