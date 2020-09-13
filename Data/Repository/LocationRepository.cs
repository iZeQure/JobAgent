using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class LocationRepository : ILocationRepository
    {
        /// <summary>
        /// Create a location.
        /// </summary>
        /// <param name="create">Used to specify the data.</param>
        public void Create(Location create)
        {
            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("CreateLocation", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters
            cmd.Parameters.AddWithValue("@name", create.Name);
            cmd.Parameters.AddWithValue("@description", create.Description);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>A list of <see cref="Location"/>.</returns>
        public IEnumerable<Location> GetAll()
        {
            // Initialize temporary list.
            List<Location> tempLocations = new List<Location>();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("GetAllLocations", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            Location location = new Location();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    tempLocations.Add(
                        new Location()
                        {
                            Id = reader.GetInt32("Id"),
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
        public Location GetById(int id)
        {
            // Initalize temporary obj.
            Location tempLocationObj = new Location();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetLocationById", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connetion to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if reader has any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    tempLocationObj.Id = reader.GetInt32("Id");
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
        public void Remove(int id)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveLocation", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connetion to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update location.
        /// </summary>
        /// <param name="update">Used to specify the data set to update.</param>
        public void Update(Location update)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateLocation", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Id);
            cmd.Parameters.AddWithValue("@name", update.Name);
            cmd.Parameters.AddWithValue("@description", update.Description);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute update.
            cmd.ExecuteNonQuery();
        }
    }
}