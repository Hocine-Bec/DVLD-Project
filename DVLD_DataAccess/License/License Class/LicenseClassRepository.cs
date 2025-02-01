using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.CountriesRepository;
using System.Net;
using System.Security.Policy;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class LicenseClassRepository
    {
        public LicenseClassDTO GetLicenseClassInfoById(int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseClassSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return LicenseClassDataMapper.MapToLicenseClassDTO(reader);
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

        public LicenseClassDTO GetLicenseClassInfoByClassName(string className)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseClassSqlStatements.GetByClassName, connection))
                {
                    command.Parameters.AddWithValue("@ClassName", className);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return LicenseClassDataMapper.MapToLicenseClassDTO(reader);
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

        public DataTable GetAllLicenseClasses()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseClassSqlStatements.GetAllLicenseClasses, connection))
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

        public int AddNewLicenseClass(LicenseClassDTO licenseClassDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseClassSqlStatements.AddNewLicenseClass, connection))
                {
                    LicenseClassParameterBuilder.FillSqlCommandParameters(command, licenseClassDTO);

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

        public bool UpdateLicenseClass(LicenseClassDTO licenseClassDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseClassSqlStatements.UpdateLicenseClass, connection))
                {
                    command.Parameters.AddWithValue("@LicenseClassID", licenseClassDTO.LicenseClassID);
                    LicenseClassParameterBuilder.FillSqlCommandParameters(command, licenseClassDTO);

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
