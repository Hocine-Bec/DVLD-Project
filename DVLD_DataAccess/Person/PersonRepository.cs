using DVLD.DTOs;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;

namespace DVLD_DataAccess
{
    public class PersonRepository 
    {
        public PersonDTO GetPersonById(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.GetByPersonId, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return PersonDataMapper.MapToPersonDTO(reader);
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

        public PersonDTO GetPersonByNationalNo(string nationalNo)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.GetByNationalNo, connection))
                {
                    command.Parameters.AddWithValue("@nationalNo", nationalNo);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return PersonDataMapper.MapToPersonDTO(reader);
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

        public int AddNewPerson(PersonDTO person)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.AddNewPerson, connection))
                {
                    PersonParameterBuilder.FillSqlCommandParameters(command, person);

                    connection.Open();
                    var result = command.ExecuteScalar();

                    return result != null && int.TryParse(result.ToString(), out var personId)
                        ? personId : -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public bool UpdatePerson(PersonDTO person)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.UpdatePerson, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", person.PersonID);
                    PersonParameterBuilder.FillSqlCommandParameters(command, person);


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

        public DataTable GetAllPeople()
        {
            var dataTable = new DataTable();

            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.GetAllPeople, connection))
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

        public bool DeletePerson(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.DeletePerson, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

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

        public bool IsPersonExist(int personId)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.IsPersonExistById, connection))
                {
                    command.Parameters.AddWithValue("@PersonID", personId);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public bool IsPersonExist(string nationalNo)
        {
            try
            {
                using (var connection = new SqlConnection(DbConfig.ConnectionString))
                using (var command = new SqlCommand(PersonSqlStatements.IsPersonExistByNationalNo, connection))
                {
                    command.Parameters.AddWithValue("@NationalNo", nationalNo);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

    }

}
