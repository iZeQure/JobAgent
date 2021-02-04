using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ConsultantAreaRepository : IConsultantAreaRepository
    {
        /// <summary>
        /// Create a consultant area.
        /// </summary>
        /// <param name="create">Used to specify the data set.</param>
        public async void Create(ConsultantArea create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("CreateConsultantArea", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@name", create.Name);

            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Get all consultant areas.
        /// </summary>
        /// <returns>A list of <see cref="ConsultantArea"/>.</returns>
        public async Task<IEnumerable<ConsultantArea>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary list.
            List<ConsultantArea> tempConsultantAreas = new List<ConsultantArea>();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetAllConsultantAreas", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read data.
                while (await reader.ReadAsync())
                {
                    tempConsultantAreas.Add(
                        new ConsultantArea()
                        {
                            Identifier = reader.GetInt32("Id"),
                            Name = reader.GetString("Name")
                        });
                }
            }

            // Return dataset.
            return tempConsultantAreas;
        }

        /// <summary>
        /// Get consultant area by id.
        /// </summary>
        /// <param name="id">Used to specify the data to return.</param>
        /// <returns>A <see cref="ConsultantArea"/>.</returns>
        public async Task<ConsultantArea> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary obj.
            ConsultantArea tempConsultantArea = new ConsultantArea();

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetConsultantAreaById", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read dataset.
                while (await reader.ReadAsync())
                {
                    tempConsultantArea.Identifier = reader.GetInt32("Id");
                    tempConsultantArea.Name = reader.GetString("Name");
                }
            }

            // Return data obj.
            return tempConsultantArea;
        }

        /// <summary>
        /// Remove consultant area.
        /// </summary>
        /// <param name="id">Used to specify which dataset to remove.</param>
        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command, catch return code.
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Update consultant area.
        /// </summary>
        /// <param name="update">Used to specify what dataset needs to update.</param>
        public async void Update(ConsultantArea update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveConsultantArea", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Identifier);
            cmd.Parameters.AddWithValue("@name", update.Name);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command, catch return code.
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
