using DVLD.DTOs;
using System;
using System.Data.SqlClient;

namespace DVLD_DataAccess
{
    public static class PersonParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, PersonDTO person)
        {
            command.Parameters.AddWithValue($"@{nameof(person.FirstName)}", person.FirstName);
            command.Parameters.AddWithValue($"@{nameof(person.SecondName)}", person.SecondName);
            command.Parameters.AddWithValue($"@{nameof(person.ThirdName)}", string.IsNullOrEmpty(person.ThirdName) ? DBNull.Value : (object)person.ThirdName);
            command.Parameters.AddWithValue($"@{nameof(person.LastName)}", person.LastName);
            command.Parameters.AddWithValue($"@{nameof(person.NationalNo)}", person.NationalNo);
            command.Parameters.AddWithValue($"@{nameof(person.DateOfBirth)}", person.DateOfBirth);
            command.Parameters.AddWithValue($"@{nameof(person.Gender)}", person.Gender);
            command.Parameters.AddWithValue($"@{nameof(person.Address)}", person.Address);
            command.Parameters.AddWithValue($"@{nameof(person.Phone)}", person.Phone);
            command.Parameters.AddWithValue($"@{nameof(person.Email)}", string.IsNullOrEmpty(person.Email) ? DBNull.Value : (object)person.Email);
            command.Parameters.AddWithValue($"@{nameof(person.CountryId)}", person.CountryId);
            command.Parameters.AddWithValue($"@{nameof(person.ImagePath)}", string.IsNullOrEmpty(person.ImagePath) ? DBNull.Value : (object)person.ImagePath);
        }
    }

}
