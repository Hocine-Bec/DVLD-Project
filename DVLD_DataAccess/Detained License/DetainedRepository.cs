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
    public class DetainedRepository
    {
        public DetainedDTO GetDetainedLicenseInfoById(int detainId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@DetainID", detainId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DetainedDataMapper.MapToDetainedLicenseDTO(reader);
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

        public DetainedDTO GetDetainedLicenseInfoByLicenseId(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.GetByLicenseId, connection))
                {
                    command.Parameters.AddWithValue("@LicenseID", licenseId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DetainedDataMapper.MapToDetainedLicenseDTO(reader);
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

        public DataTable GetAllDetainedLicenses()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.GetAll, connection))
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

        public int AddNewDetainedLicense(DetainedDTO dto)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.AddNew, connection))
                {
                    DetainedParameterBuilder.FillSqlCommandParameters(command, dto);

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

        public bool UpdateDetainedLicense(DetainedDTO dto)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.Update, connection))
                {
                    DetainedParameterBuilder.FillSqlCommandParameters(command, dto);
                    command.Parameters.AddWithValue("@DetainID", dto.DetainId);

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

        public bool ReleaseDetainedLicense(int detainId, int releasedByUserId, int releaseApplicationId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.Release, connection))
                {
                    DetainedParameterBuilder.FillSqlCommandParameters(command, detainId, releasedByUserId, releaseApplicationId);

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

        public bool IsLicenseDetained(int licenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DetainedSqlStatements.IsLicenseDetained, connection))
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
