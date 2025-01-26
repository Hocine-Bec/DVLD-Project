using System;
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
    public class DetainedLicenseRepository
    {
        public static DetainedLicensesDTO GetDetainedLicenseInfoById(int detainId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", detainId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DetainedLicenseDataMapper.MapToDetainedLicenseDTO(reader);
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

        public static DetainedLicensesDTO GetDetainedLicenseInfoByLicenseId(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.GetByLicenseId, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DetainedLicenseDataMapper.MapToDetainedLicenseDTO(reader);
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

        public static DataTable GetAllDetainedLicenses()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.GetAll, connection))
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

        public static int AddNewDetainedLicense(int licenseId, DateTime detainDate, float fineFees, int createdByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.AddNew, connection))
                {
                    DetainedLicenseParameterBuilder.FillSqlCommandParameters(command, licenseId, detainDate, fineFees, createdByUserId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var insertedId)
                        ? insertedId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static bool UpdateDetainedLicense(int detainId, int licenseId, DateTime detainDate, float fineFees, int createdByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.Update, connection))
                {
                    DetainedLicenseParameterBuilder.FillSqlCommandParameters(command, licenseId, detainDate, fineFees, createdByUserId);
                    command.Parameters.AddWithValue("@DetainID", detainId);

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

        public static bool ReleaseDetainedLicense(int detainId, int releasedByUserId, int releaseApplicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.Release, connection))
                {
                    DetainedLicenseParameterBuilder.FillSqlCommandParameters(command, detainId, releasedByUserId, releaseApplicationId);

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

        public static bool IsLicenseDetained(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedLicenseSqlStatements.IsLicenseDetained, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();
                    var result = command.ExecuteScalar();
                    return result != null;
                }
            }
            catch
            {
                return false;
            }
        }
    }

}
