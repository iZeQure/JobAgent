using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        /// <summary>
        /// Create a location.
        /// </summary>
        /// <param name="create">Used to specify the data.</param>
        public async void Create(Location create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("CreateLocation", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters
            cmd.Parameters.AddWithValue("@name", create.Name);
            cmd.Parameters.AddWithValue("@description", create.Description);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>A list of <see cref="Location"/>.</returns>
        public async Task<IEnumerable<Location>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary list.
            List<Location> tempLocations = new List<Location>();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("GetAllLocations", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            Location location = new Location();

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

        /// <summary>
        /// Get location by id.
        /// </summary>
        /// <param name="id">Used to identity the data to get.</param>
        /// <returns>A <see cref="Location"/> if exists.</returns>
        public async Task<Location> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initalize temporary obj.
            Location tempLocationObj = new Location();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetLocationById", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connetion to database.
            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check if reader has any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (await reader.ReadAsync())
                {
                    tempLocationObj.Identifier = reader.GetInt32("Id");
                    tempLocationObj.Name = reader.GetString("Name");

                    if (!DataReaderExtensions.IsDBNull(reader, "@description"))
                        tempLocationObj.Description = reader.GetString("Description");
                    else
                        tempLocationObj.Description = string.Empty;
                }
            }

            // Return data obj.
            return tempLocationObj;
        }

        /// <summary>
        /// Remove location by id.
        /// </summary>
        /// <param name="id">Used to specify the data to remove.</param>
        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveLocation", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connetion to database.
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Update location.
        /// </summary>
        /// <param name="update">Used to specify the data set to update.</param>
        public async void Update(Location update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateLocation", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Identifier);
            cmd.Parameters.AddWithValue("@name", update.Name);
            cmd.Parameters.AddWithValue("@description", update.Description);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute update.
            await cmd.ExecuteNonQueryAsync();
        }
    }
}