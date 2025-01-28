using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DVLD_DataAccess.CountriesRepository;
using System.Net;
using System.Security.Policy;
using System.ComponentModel;
using DVLD.DTOs;

namespace DVLD_DataAccess
{
    public class LicenseRepository
    {
        public static LicenseDTO GetLicenseInfoById(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return LicenseDataMapper.MapToLicenseDTO(reader);
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

        public static DataTable GetAllLicenses()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.GetAll, connection))
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

        public static DataTable GetDriverLicenses(int driverId)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.GetDriverLicenses, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);

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

        public static int AddNewLicense(LicenseDTO licenseDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.AddNew, connection))
                {
                    LicenseParameterBuilder.FillSqlCommandParameters(command, licenseDTO);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var licenseId)
                        ? licenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateLicense(LicenseDTO licenseDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.Update, connection))
                {
                    LicenseParameterBuilder.FillSqlCommandParameters(command, licenseDTO, licenseDTO.LicenseID);

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

        public static int GetActiveLicenseIdByPersonId(int personId, int licenseClassId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.GetActiveLicenseIdByPersonId, connection))
                {
                    LicenseParameterBuilder.FillSqlCommandParameters(command, personId, licenseClassId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var licenseId)
                        ? licenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool DeactivateLicense(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(LicenseSqlStatements.Deactivate, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

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
