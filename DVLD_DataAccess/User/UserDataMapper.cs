using DVLD.DTOs;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class UserDataMapper
    {
        public static UsersDTO MapToUserDTO(SqlDataReader reader)
        {
            return new UsersDTO
            {
                UserID = (int)reader["UserID"],
                PersonID = (int)reader["PersonID"],
                Username = (string)reader["UserName"],
                Password = (string)reader["Password"],
                IsActive = (bool)reader["IsActive"]
            };
        }
    }
}
