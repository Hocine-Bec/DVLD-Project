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
    public class InternationalRepository
    {
        public InternationalDTO GetInternationalLicenseById(int internationalLicenseId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.GetById, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", internationalLicenseId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return InternationalDataMapper.MapToInternationalLicenseDTO(reader);
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

        public DataTable GetAllInternationalLicenses()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.GetAll, connection))
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

        public DataTable GetDriverInternationalLicenses(int driverId)
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.GetDriverInternationalLicenses, connection))
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

        public int AddNewInternationalLicense(InternationalDTO internationalLicenseDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.AddNew, connection))
                {
                    InternationalParameterBuilder.FillSqlCommandParameters(command, internationalLicenseDTO);

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

        public bool UpdateInternationalLicense(InternationalDTO internationalLicenseDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.Update, connection))
                {
                    InternationalParameterBuilder.FillSqlCommandParameters
                        (command, internationalLicenseDTO, internationalLicenseDTO.InternationalId);

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

        public int GetActiveInternationalLicenseIdByDriverId(int driverId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(InternationalSqlStatements.GetActiveInternationalLicenseIdByDriverId, connection))
                {
                    command.Parameters.AddWithValue("@InternationalLicenseID", driverId);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var internationalLicenseId)
                        ? internationalLicenseId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }
    }

}
