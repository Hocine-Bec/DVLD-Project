using System;
using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestTypeDataMapper
    {
        public static TestTypesDTO MapToTestTypeDTO(SqlDataReader reader)
        {
            return new TestTypesDTO
            {
                TestTypeID = (int)reader["TestTypeID"],
                Title = (string)reader["TestTypeTitle"],
                Description = (string)reader["TestTypeDescription"],
                Fees = Convert.ToSingle(reader["TestTypeFees"]),
            };
        }
    }

}
