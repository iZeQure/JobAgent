using JobAgent.Data.DB;
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
    public class ConsultantAreaRepository : IConsultantAreaRepository
    {
        /// <summary>
        /// Create a consultant area.
        /// </summary>
        /// <param name="create">Used to specify the data set.</param>
        public void Create(ConsultantArea create)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("CreateConsultantArea", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@name", create.Name);

            cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all consultant areas.
        /// </summary>
        /// <returns>A list of <see cref="ConsultantArea"/>.</returns>
        public IEnumerable<ConsultantArea> GetAll()
        {
            // Initialize temporary list.
            List<ConsultantArea> tempConsultantAreas = new List<ConsultantArea>();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetAllConsultantAreas", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempConsultantAreas.Add(
                        new ConsultantArea()
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1)
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
        public ConsultantArea GetById(int id)
        {
            // Initialize temporary obj.
            ConsultantArea tempConsultantArea = new ConsultantArea();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetConsultantAreaById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read dataset.
                while (reader.Read())
                {
                    tempConsultantArea.Id = reader.GetInt32(0);
                    tempConsultantArea.Name = reader.GetString(1);
                }
            }

            // Return data obj.
            return tempConsultantArea;
        }

        /// <summary>
        /// Remove consultant area.
        /// </summary>
        /// <param name="id">Used to specify which dataset to remove.</param>
        public void Remove(int id)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute command, catch return code.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update consultant area.
        /// </summary>
        /// <param name="update">Used to specify what dataset needs to update.</param>
        public void Update(ConsultantArea update)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveConsultantArea", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Id);
            cmd.Parameters.AddWithValue("@name", update.Name);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute command, catch return code.
            cmd.ExecuteNonQuery();
        }
    }
}
