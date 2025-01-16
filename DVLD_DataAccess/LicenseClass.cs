using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.clsCountryData;
using System.Net;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        public static bool GetLicenseClassInfoById(int licenseClassId, ref string className,
           ref string classDescription, ref byte minimumAllowedAge, ref byte defaultValidityLength,
           ref float classFees)
        {
            const string query = "SELECT * FROM LicenseClasses WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            className = (string)reader["ClassName"];
                            classDescription = (string)reader["ClassDescription"];
                            minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            defaultValidityLength = (byte)reader["DefaultValidityLength"];
                            classFees = Convert.ToSingle(reader["ClassFees"]);
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool GetLicenseClassInfoByClassName(string className, ref int licenseClassId,
            ref string classDescription, ref byte minimumAllowedAge, ref byte defaultValidityLength,
            ref float classFees)
        {
            const string query = "SELECT * FROM LicenseClasses WHERE ClassName = @ClassName";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            licenseClassId = (int)reader["LicenseClassID"];
                            classDescription = (string)reader["ClassDescription"];
                            minimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                            defaultValidityLength = (byte)reader["DefaultValidityLength"];
                            classFees = Convert.ToSingle(reader["ClassFees"]);
                            return true;
                        }

                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static DataTable GetAllLicenseClasses()
        {
            const string query = "SELECT * FROM LicenseClasses ORDER BY ClassName";
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            dataTable.Load(reader);
                        }
                    }
                }
            }
            catch
            {
                dataTable.Clear();
            }

            return dataTable;
        }

        public static int AddNewLicenseClass(string className, string classDescription,
            byte minimumAllowedAge, byte defaultValidityLength, float classFees)
        {
            const string query = @"
            INSERT INTO LicenseClasses 
            (
                ClassName, ClassDescription, MinimumAllowedAge, 
                DefaultValidityLength, ClassFees
            )
            VALUES 
            (
                @ClassName, @ClassDescription, @MinimumAllowedAge, 
                @DefaultValidityLength, @ClassFees
            );
            SELECT SCOPE_IDENTITY();";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);
                    command.Parameters.AddWithValue("@ClassDescription", classDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", minimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", defaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", classFees);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var licenseClassId)
                        ? licenseClassId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateLicenseClass(int licenseClassId, string className,
            string classDescription, byte minimumAllowedAge, byte defaultValidityLength, float classFees)
        {
            const string query = @"
            UPDATE LicenseClasses  
            SET 
                ClassName = @ClassName,
                ClassDescription = @ClassDescription,
                MinimumAllowedAge = @MinimumAllowedAge,
                DefaultValidityLength = @DefaultValidityLength,
                ClassFees = @ClassFees
            WHERE LicenseClassID = @LicenseClassID";

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);
                    command.Parameters.AddWithValue("@ClassName", className);
                    command.Parameters.AddWithValue("@ClassDescription", classDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", minimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", defaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", classFees);

                    connection.Open();
                    var rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}
