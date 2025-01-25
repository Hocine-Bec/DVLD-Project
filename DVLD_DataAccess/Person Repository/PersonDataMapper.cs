using DVLD.DTOs;
using System;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class PersonDataMapper
    {
        public static PersonDTO MapToPersonDTO(SqlDataReader reader)
        {
            return new PersonDTO
            {
                PersonID = (int)reader["PersonID"],
                FirstName = (string)reader["FirstName"],
                SecondName = (string)reader["SecondName"],
                ThirdName = reader["ThirdName"] == DBNull.Value ? "" : (string)reader["ThirdName"],
                LastName = (string)reader["LastName"],
                NationalNo = (string)reader["NationalNo"],
                DateOfBirth = (DateTime)reader["DateOfBirth"],
                Gender = (byte)reader["Gender"],
                Address = (string)reader["Address"],
                Phone = (string)reader["Phone"],
                Email = reader["Email"] == DBNull.Value ? "" : (string)reader["Email"],
                NationalityCountryID = (int)reader["NationalityCountryID"],
                ImagePath = reader["ImagePath"] == DBNull.Value ? "" : (string)reader["ImagePath"]
            };
        }
    }

}
