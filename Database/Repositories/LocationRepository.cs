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
    public class LocationRepository : ILocationRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public LocationRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create a location.
        /// </summary>
        /// <param name="create">Used to specify the data.</param>
        public async Task Create(Location create)
        {
            try
            {
                // Prepare command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("CreateLocation", CommandType.StoredProcedure);

                // Define input parameters
                cmd.Parameters.AddWithValue("@name", create.Name);
                cmd.Parameters.AddWithValue("@description", create.Description);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>A list of <see cref="Location"/>.</returns>
        public async Task<IEnumerable<Location>> GetAll()
        {
            try
            {
                // Initialize temporary list.
                List<Location> tempLocations = new List<Location>();

                // Prepare command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetAllLocations", CommandType.StoredProcedure);

                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check if any data.
                if (reader.HasRows)
                {
                    // Read the data.
                    while (await reader.ReadAsync())
                    {
                        tempLocations.Add(
                            new Location()
                            {
                                Identifier = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Description = !DataReaderExtensions.IsDBNull(reader, "Description") ? reader.GetString("Description") : string.Empty
                            });
                    }
                }

                // Return data.
                return tempLocations;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get location by id.
        /// </summary>
        /// <param name="id">Used to identity the data to get.</param>
        /// <returns>A <see cref="Location"/> if exists.</returns>
        public async Task<Location> GetById(int id)
        {
            // Initalize temporary obj.
            Location tempLocationObj = new Location();

            // Initialize command obj.
            using SqlCommand cmd = _databaseAccess.GetCommand("GetLocationById", CommandType.StoredProcedure);

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Initialize data reader.
            using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

            // Check if reader has any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (await reader.ReadAsync())
                {
                    tempLocationObj.Identifier = reader.GetInt32("Id");
                    tempLocationObj.Name = reader.GetString("Name");
                    tempLocationObj.Description = !DataReaderExtensions.IsDBNull(reader, "@description") ? reader.GetString("Description") : string.Empty;
                }
            }

            // Return data obj.
            return tempLocationObj;
        }

        /// <summary>
        /// Remove location by id.
        /// </summary>
        /// <param name="id">Used to specify the data to remove.</param>
        public async Task Remove(int id)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("RemoveLocation", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connetion to database.
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
        /// Update location.
        /// </summary>
        /// <param name="update">Used to specify the data set to update.</param>
        public async Task Update(Location update)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("UpdateLocation", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier);
                cmd.Parameters.AddWithValue("@name", update.Name);
                cmd.Parameters.AddWithValue("@description", update.Description);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute update.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}