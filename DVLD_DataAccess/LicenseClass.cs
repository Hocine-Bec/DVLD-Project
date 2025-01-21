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
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {
        public static LicenseClassDTO GetLicenseClassInfoById(int licenseClassId)
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
                            return new LicenseClassDTO
                            {
                                ClassName = (string)reader["ClassName"],
                                ClassDescription = (string)reader["ClassDescription"],
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"],
                                ClassFees = Convert.ToSingle(reader["ClassFees"]),
                            };
                        }

                        return null;
                    }
                }
            }
            catch
            {
                return null;
            }
        }

        public static LicenseClassDTO GetLicenseClassInfoByClassName(string className)
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
                            return new LicenseClassDTO
                            {
                                ClassName = (string)reader["ClassName"],
                                ClassDescription = (string)reader["ClassDescription"],
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"],
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"],
                                ClassFees = Convert.ToSingle(reader["ClassFees"]),
                            };
                        }

                        return null;
                    }
                }
            }
            catch
            {
                return null;
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

        public static int AddNewLicenseClass(LicenseClassDTO licenseClassDTO)
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
                    command.Parameters.AddWithValue("@ClassName", licenseClassDTO.ClassName);
                    command.Parameters.AddWithValue("@ClassDescription", licenseClassDTO.ClassDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", licenseClassDTO.MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", licenseClassDTO.DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", licenseClassDTO.ClassFees);

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

        public static bool UpdateLicenseClass(LicenseClassDTO licenseClassDTO)
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
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassDTO.LicenseClassID);
                    command.Parameters.AddWithValue("@ClassName", licenseClassDTO.ClassName);
                    command.Parameters.AddWithValue("@ClassDescription", licenseClassDTO.ClassDescription);
                    command.Parameters.AddWithValue("@MinimumAllowedAge", licenseClassDTO.MinimumAllowedAge);
                    command.Parameters.AddWithValue("@DefaultValidityLength", licenseClassDTO.DefaultValidityLength);
                    command.Parameters.AddWithValue("@ClassFees", licenseClassDTO.ClassFees);

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
