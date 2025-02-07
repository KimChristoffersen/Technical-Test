using System.Data.SqlClient;
using TekniskOpgave.DAL.Interfaces;
using TekniskOpgave.DAL.Models;
using Microsoft.Extensions.Configuration;

namespace TekniskOpgave.DAL.SQL
{
    public class MontorerSqlDataAccess : IMontorerSqlDataAccess
    {
        private readonly string _connectionString;

        public MontorerSqlDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<Montor> GetAllMontors()
        {
            string queryString = "SELECT * FROM Montorer";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);

            try
            {
                connection.Open();
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
                throw new Exception("ERROR occurred while getting montor", ex);
            }
        }


        public Montor GetMontorById(int montorId)
        {
            Montor montor = new Montor() { MontorId = 0, MontorNavn = "", MontorTelefonnummer = "" };
            string queryString = @"SELECT * FROM Montorer WHERE montor_Id = @Id";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@Id", montorId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    montor.MontorId = (int)reader["montor_Id"];
                    montor.MontorNavn = (string)reader["navn"];
                    montor.MontorTelefonnummer = (string)reader["telefonnummer"];
                    montor.Overmontorer = GetOvermontorerForMontor(montorId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR occurred while getting montor", ex);
            }
            return montor;
        }


        private List<Overmontor> GetOvermontorerForMontor(int montorId)
        {
            List<Overmontor> overmontorer = new List<Overmontor>();
            string queryString = @"SELECT o.* FROM Overmontorer o
                            JOIN MontorOvermontorReference m ON m.overmontor_Id = o.overmontor_Id
                            WHERE m.montor_Id = @MontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@MontorId", montorId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    overmontorer.Add(new Overmontor
                    {
                        OvermontorId = (int)reader["overmontor_Id"],
                        OvermontorNavn = (string)reader["navn"],
                        OvermontorTelefonnummer = (string)reader["telefonnummer"]
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR occurred while getting overmontorer", ex);
            }

            return overmontorer;
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


        public void CreateMontor(Montor montor, int? overmontorId)
        {
            string query = "INSERT INTO Montorer (navn, telefonnummer) VALUES (@Navn, @Telefonnummer); SELECT SCOPE_IDENTITY();";
            string relationQuery = "INSERT INTO MontorOvermontorReference (Montor_Id, Overmontor_Id) VALUES (@MontorId, @OvermontorId)";

            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            using SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                using SqlCommand command = new SqlCommand(query, connection, transaction);
                command.Parameters.AddWithValue("@Navn", montor.MontorNavn);
                command.Parameters.AddWithValue("@Telefonnummer", montor.MontorTelefonnummer);

                int montorId = Convert.ToInt32(command.ExecuteScalar());

                if (overmontorId.HasValue)
                {
                    using SqlCommand relationCommand = new SqlCommand(relationQuery, connection, transaction);
                    relationCommand.Parameters.AddWithValue("@MontorId", montorId);
                    relationCommand.Parameters.AddWithValue("@OvermontorId", overmontorId.Value);

                    relationCommand.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new Exception("Error creating montor", ex);
            }
        }


        public void DeleteMontor(int montorId)
        {
            string query = "DELETE FROM Montorer WHERE montor_Id = @MontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MontorId", montorId);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting montor", ex);
            }
        }


        public void UpdateMontor(Montor montor)
        {
            string query = "UPDATE Montorer SET navn = @Navn, telefonnummer = @Telefonnummer WHERE montor_Id = @MontorId";
            using SqlConnection connection = new SqlConnection(_connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@MontorId", montor.MontorId);
            command.Parameters.AddWithValue("@Navn", montor.MontorNavn);
            command.Parameters.AddWithValue("@Telefonnummer", montor.MontorTelefonnummer);

            try
            {
            connection.Open();
            command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating montor", ex);
            }
        }
    }
}
