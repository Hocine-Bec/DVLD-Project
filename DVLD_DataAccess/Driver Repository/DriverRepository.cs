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
    public class DriverRepository
    {
        public static DriverDTO GetDriverInfoByDriverId(int driverId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DriverSqlStatements.GetByDriverId, connection))
                {
                    command.Parameters.AddWithValue("@DriverID", driverId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DriverDataMapper.MapToDriverInfo(reader);
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

        public static DriverDTO GetDriverInfoByPersonId(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DriverSqlStatements.GetByPersonId, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return DriverDataMapper.MapToDriverInfo(reader);
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

        public static DataTable GetAllDrivers()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DriverSqlStatements.GetAll, connection))
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

        public static int AddNewDriver(int personId, int createdByUserId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DriverSqlStatements.AddNew, connection))
                {
                    DriverParameterBuilder.FillSqlCommandParameters(command, personId, createdByUserId);

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

        public static bool UpdateDriver(DriverDTO driverDTO)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(DriverSqlStatements.Update, connection))
                {
                    DriverParameterBuilder.FillSqlCommandParameters(command, driverDTO);

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
