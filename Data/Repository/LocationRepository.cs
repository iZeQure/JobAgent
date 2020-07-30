﻿using JobAgent.Data.DB;
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
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("CreateLocation", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters
            cmd.Parameters.AddWithValue("Name", create.Name);
            cmd.Parameters.AddWithValue("Description", create.Description);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all locations.
        /// </summary>
        /// <returns>A list of <see cref="Location"/>.</returns>
        public IEnumerable<Location> GetAll()
        {
            // Initialize temporary list.
            List<Location> tempLocations = new List<Location>();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("GetAllLocations", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    // Store data in the temporary list.
                    tempLocations.Add(new Location()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
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

            // Open connetion to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetLocationById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("LocationId", id);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if reader has any data.
            if (reader.HasRows)
            {
                // Read the data.
                while (reader.Read())
                {
                    tempLocationObj.Id = reader.GetInt32(0);
                    tempLocationObj.Name = reader.GetString(1);
                    tempLocationObj.Description = reader.GetString(2);
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
            // Open connetion to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveLocation", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("LocationId", id);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update location.
        /// </summary>
        /// <param name="update">Used to specify the data set to update.</param>
        public void Update(Location update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateLocation", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("LocationId", update.Id);
            cmd.Parameters.AddWithValue("NewLocationName", update.Name);
            cmd.Parameters.AddWithValue("NewLocationDescription", update.Description);

            // Execute update, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }
    }
}