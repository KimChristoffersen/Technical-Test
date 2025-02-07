using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TekniskOpgave.DAL.Interfaces;
using TekniskOpgave.DAL.Models;

namespace TekniskOpgave.DAL.SQL
{
    public class OvermontorerSqlDataAccess : IOvermontorerSqlDataAccess
    {
        private readonly string _connectionString;

        public OvermontorerSqlDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Overmontor> GetAllOvermontors()
        {
            string queryString = "SELECT * FROM Overmontorer";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
                List<Overmontor> overmontorer = new List<Overmontor>();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    overmontorer.Add(new Overmontor
                    {
                        OvermontorId = (int)reader["overmontor_Id"],
                        OvermontorNavn = reader["navn"].ToString(),
                        OvermontorTelefonnummer = reader["telefonnummer"].ToString()
                    });
                }

                return overmontorer;
            }
            catch (Exception ex)
            {
                throw new Exception("Error occurred while fetching all overmontors.", ex);
            }
        }


        public Overmontor GetOvermontorById(int overmontorId)
        {
            string queryString = "SELECT * FROM Overmontorer WHERE overmontor_Id = @OvermontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@OvermontorId", overmontorId);

            try
            {
                connection.Open();
                Overmontor overmontor = null;
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    overmontor = new Overmontor
                    {
                        OvermontorId = (int)reader["overmontor_Id"],
                        OvermontorNavn = reader["navn"].ToString(),
                        OvermontorTelefonnummer = reader["telefonnummer"].ToString(),
                        Montorer = GetMontorerForOvermontor(overmontorId)
                    };
                }

                return overmontor;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching overmontor with ID {overmontorId}.", ex);
            }
        }


        private List<Montor> GetMontorerForOvermontor(int overmontorId)
        {
            string queryString = @"SELECT m.* FROM Montorer m
                                   INNER JOIN MontorOvermontorReference mor ON m.montor_Id = mor.montor_Id
                                   WHERE mor.overmontor_Id = @OvermontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@OvermontorId", overmontorId);

            try
            {
                connection.Open(); // Flyttet inden for try-catch
                List<Montor> montorer = new List<Montor>();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    montorer.Add(new Montor
                    {
                        MontorId = (int)reader["montor_Id"],
                        MontorNavn = reader["navn"].ToString(),
                        MontorTelefonnummer = reader["telefonnummer"].ToString()
                    });
                }
                return montorer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching montors for overmontor with ID {overmontorId}.", ex);
            }
        }


        public void AddMontorToOvermontor(int montorId, int overmontorId)
        {
            string queryString = "INSERT INTO MontorOvermontorReference (montor_Id, overmontor_Id) VALUES (@MontorId, @OvermontorId)";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@MontorId", montorId);
            command.Parameters.AddWithValue("@OvermontorId", overmontorId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding montor to overmontor", ex);
            }
        }


        public void RemoveMontorFromOvermontor(int montorId, int overmontorId)
        {
            string queryString = "DELETE FROM MontorOvermontorReference WHERE montor_Id = @MontorId AND overmontor_Id = @OvermontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@MontorId", montorId);
            command.Parameters.AddWithValue("@OvermontorId", overmontorId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error removing montor from overmontor", ex);
            }
        }


        public void CreateOvermontor(Overmontor overmontor)
        {
            string query = "INSERT INTO Overmontorer (navn, telefonnummer) VALUES (@Navn, @Telefonnummer)";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Navn", overmontor.OvermontorNavn);
            command.Parameters.AddWithValue("@Telefonnummer", overmontor.OvermontorTelefonnummer);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating overmontor", ex);
            }
        }


        public void DeleteOvermontor(int overmontorId)
        {
            string query = "DELETE FROM Overmontorer WHERE overmontor_Id = @OvermontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OvermontorId", overmontorId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting overmontor", ex);
            }
        }


        public void UpdateOvermontor(Overmontor overmontor)
        {
            string query = "UPDATE Overmontorer SET navn = @Navn, telefonnummer = @Telefonnummer WHERE overmontor_Id = @OvermontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@OvermontorId", overmontor.OvermontorId);
            command.Parameters.AddWithValue("@Navn", overmontor.OvermontorNavn);
            command.Parameters.AddWithValue("@Telefonnummer", overmontor.OvermontorTelefonnummer);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception("Error updating overmontor", ex);
            }
        }
    }
}