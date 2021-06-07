using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.SqlAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class ConsultantAreaRepository : IConsultantAreaRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public ConsultantAreaRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create a consultant area.
        /// </summary>
        /// <param name="create">Used to specify the data set.</param>
        public async Task Create(ConsultantArea create)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("CreateConsultantArea", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@name", create.Name);

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all consultant areas.
        /// </summary>
        /// <returns>A list of <see cref="ConsultantArea"/>.</returns>
        public async Task<IEnumerable<ConsultantArea>> GetAll()
        {
            try
            {
                // Initialize temporary list.
                List<ConsultantArea> tempConsultantAreas = new List<ConsultantArea>();

                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetAllConsultantAreas", CommandType.StoredProcedure);

                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

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
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get consultant area by id.
        /// </summary>
        /// <param name="id">Used to specify the data to return.</param>
        /// <returns>A <see cref="ConsultantArea"/>.</returns>
        public async Task<ConsultantArea> GetById(int id)
        {
            try
            {
                // Initialize temporary obj.
                ConsultantArea tempConsultantArea = new ConsultantArea();

                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetConsultantAreaById", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

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
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Remove consultant area.
        /// </summary>
        /// <param name="id">Used to specify which dataset to remove.</param>
        public async Task Remove(int id)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("RemoveConsultantArea", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command, catch return code.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Update consultant area.
        /// </summary>
        /// <param name="update">Used to specify what dataset needs to update.</param>
        public async Task Update(ConsultantArea update)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("RemoveConsultantArea", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier);
                cmd.Parameters.AddWithValue("@name", update.Name);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command, catch return code.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
