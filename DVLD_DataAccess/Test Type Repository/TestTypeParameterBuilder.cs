using System.Data.SqlClient;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public static class TestTypeParameterBuilder
    {
        public static void FillSqlCommandParameters(SqlCommand command, TestTypesDTO testType)
        {
            command.Parameters.AddWithValue($"@{nameof(testType.TestTypeID)}", testType.TestTypeID);
            command.Parameters.AddWithValue($"@{nameof(testType.Title)}", testType.Title);
            command.Parameters.AddWithValue($"@{nameof(testType.Description)}", testType.Description);
            command.Parameters.AddWithValue($"@{nameof(testType.Fees)}", testType.Fees);
        }
    }

}
