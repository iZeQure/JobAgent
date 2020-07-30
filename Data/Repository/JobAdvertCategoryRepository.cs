using JobAgent.Data.Repository.Interface;
using JobAgent.Data.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace JobAgent.Data.Repository
{
    public class JobAdvertCategoryRepository : IJobAdvertCategoryRepository
    {
        /// <summary>
        /// Create a job advert category.
        /// </summary>
        /// <param name="create">Used to specify the data for the category.</param>
        public void Create(JobAdvertCategory create)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateJobAdvertCategory", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("CategoryName", create.Name);
            cmd.Parameters.AddWithValue("CategoryDescription", create.Description);

            // Execute the command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a job advert category specialization.
        /// </summary>
        /// <param name="create">Used to specify the data for the specialization.</param>
        /// <param name="categoryId">Used to bind the specialization to a specific category.</param>
        public void CreateJobAdvertCategorySpecialization(JobAdvertCategorySpecialization create, int categoryId)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateJobAdvertCategorySpecialization", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("CategoryID", categoryId);
            cmd.Parameters.AddWithValue("SpecializationName", create.Name);
            cmd.Parameters.AddWithValue("SpecializationDescription", create.Description);

            // Exectue the command, catch the return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>A list of <see cref="JobAdvertCategory"/></returns>
        public IEnumerable<JobAdvertCategory> GetAll()
        {
            // Initialize a temporary list with categories.
            List<JobAdvertCategory> tempCategories = new List<JobAdvertCategory>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetAllJobAdvertCategories",
                Connection = Database.Instance.SqlConnection
            };

            // Initialize Data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if the reader has rows.
            if (reader.HasRows)
            {
                // Add data to the temp list while reading the data.
                while (reader.Read())
                {
                    tempCategories.Add(new JobAdvertCategory()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2)
                    });
                }
            }

            // return data.
            return tempCategories;
        }

        /// <summary>
        /// Get all categories with associated specializations.
        /// </summary>
        /// <returns>A list of <see cref="JobAdvertCategory" with a list of <see cref="JobAdvertCategory.JobAdvertCategorySpecializations"/>/></returns>
        public List<JobAdvertCategory> GetAllJobAdvertCategoriesWithSpecializations()
        {
            // Initialize a temporary list with categories.
            List<JobAdvertCategory> tempJobAdvertCategories = new List<JobAdvertCategory>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare sql command object with information.
            using SqlCommand cmd = new SqlCommand("GetAllJobAdvertCategoriesWithSpecialization", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Initialize a data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if the reader has any rows.
            if (reader.HasRows)
            {
                // Add data to the temporary list while reading.
                while (reader.Read())
                {
                    // Initialize temporary objects to store the data received from the database.
                    var jobAdvertCategory = new JobAdvertCategory();
                    var jobAdvertCategorySpecialization = new JobAdvertCategorySpecialization();

                    // Check if the values from the database contains null.
                    if (!DataReaderExtensions.IsDBNull(reader, "Id")) jobAdvertCategory.Id = reader.GetInt32(0);
                    if (!DataReaderExtensions.IsDBNull(reader, "CategoryName")) jobAdvertCategory.Name = reader.GetString(1);
                    if (!DataReaderExtensions.IsDBNull(reader, "CategoryDescription")) jobAdvertCategory.Description = reader.GetString(2);
                    if (!DataReaderExtensions.IsDBNull(reader, "JobAdvertCategoryId")) jobAdvertCategorySpecialization.Id = reader.GetInt32(3);
                    if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobAdvertCategorySpecialization.Name = reader.GetString(4);
                    if (!DataReaderExtensions.IsDBNull(reader, "SpecializationDescription")) jobAdvertCategorySpecialization.Description = reader.GetString(5);

                    // If the Category ID Matches the specialization associated ID.
                    if (jobAdvertCategory.Id == jobAdvertCategorySpecialization.Id)
                    {
                        // Check if the category exists in the list.
                        if (tempJobAdvertCategories.Any(id => id.Id == jobAdvertCategory.Id))
                        {
                            // Loop through category list.
                            foreach (var category in tempJobAdvertCategories)
                            {
                                // Check if the specialization id matches any category id's in the list.
                                if (category.Id == jobAdvertCategorySpecialization.Id)
                                {
                                    // Add specialization to the category with the associated list of specializations.
                                    category.JobAdvertCategorySpecializations.Add(
                                    new JobAdvertCategorySpecialization()
                                    {
                                        Id = jobAdvertCategorySpecialization.Id,
                                        Name = jobAdvertCategorySpecialization.Name,
                                        Description = jobAdvertCategorySpecialization.Description
                                    });
                                }
                            }
                        }
                        else
                        {
                            // Add Category with specialization if not exists in the current temporary list.
                            tempJobAdvertCategories.Add(new JobAdvertCategory()
                            {
                                Id = jobAdvertCategory.Id,
                                Name = jobAdvertCategory.Name,
                                Description = jobAdvertCategory.Description,
                                JobAdvertCategorySpecializations = new List<JobAdvertCategorySpecialization>()
                                    {
                                        new JobAdvertCategorySpecialization()
                                        {
                                            Id = jobAdvertCategorySpecialization.Id,
                                            Name = jobAdvertCategorySpecialization.Name,
                                            Description = jobAdvertCategorySpecialization.Description
                                        }
                                    }
                            });
                        }
                    }
                    else
                    {
                        // Add new category if not exists.
                        tempJobAdvertCategories.Add(new JobAdvertCategory()
                        {
                            Id = jobAdvertCategory.Id,
                            Name = jobAdvertCategory.Name,
                            Description = jobAdvertCategory.Description,
                            JobAdvertCategorySpecializations = null
                        });
                    }
                }
            }

            // Dispose the reader.
            reader.Close();

            // return data.
            return tempJobAdvertCategories;
        }

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>A list of <see cref="JobAdvertCategorySpecialization"/>.</returns>
        public List<JobAdvertCategorySpecialization> GetAllJobAdvertCategorySpecializations()
        {
            // Initialize a temporary list with specializations.
            List<JobAdvertCategorySpecialization> tempSpecializations = new List<JobAdvertCategorySpecialization>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command with information to execute.
            using SqlCommand cmd = new SqlCommand("GetAllJobAdvertCategorySpecializations", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any rows.
            if (reader.HasRows)
            {
                // Add data to temporary list while reading.
                while (reader.Read())
                {
                    tempSpecializations.Add(new JobAdvertCategorySpecialization()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.GetString(2),
                        JobAdvertCategoryId = new JobAdvertCategory()
                        {
                            Id = reader.GetInt32(3)
                        }
                    });
                }
            }

            // Return data
            return tempSpecializations;
        }

        /// <summary>
        /// Get category by id.
        /// </summary>
        /// <param name="id">Used to indicate which category to return.</param>
        /// <returns>A <see cref="JobAdvertCategory"/>.</returns>
        public JobAdvertCategory GetById(int id)
        {
            // Initalize temporary category object.
            JobAdvertCategory tempCategory = new JobAdvertCategory();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("GetJobAdvertCategoryById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("CategoryId", id);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any rows.
            if (reader.HasRows)
            {
                // Get the row specified by the id.
                tempCategory.Id = reader.GetInt32(0);
                tempCategory.Name = reader.GetString(1);
                tempCategory.Description = reader.GetString(2);
            }

            // return data
            return tempCategory;
        }

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="id">Used to identify the specialization to return.</param>
        /// <returns>A <see cref="JobAdvertCategorySpecialization"/>.</returns>
        public JobAdvertCategorySpecialization GetJobAdvertCategorySpecializationById(int id)
        {
            // Initialize temporary data object.
            JobAdvertCategorySpecialization tempSpecializationObj = new JobAdvertCategorySpecialization();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command with information.
            using SqlCommand cmd = new SqlCommand("GetJobAdvertCategorySpecializationById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add specialization to get.
            cmd.Parameters.AddWithValue("SpecializationId", id);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for rows
            if (reader.HasRows)
            {
                // Read the data
                while (reader.Read())
                {
                    tempSpecializationObj.Id = reader.GetInt32(0);
                    tempSpecializationObj.Name = reader.GetString(1);
                    tempSpecializationObj.Description = reader.GetString(2);
                }
            }

            // return data obj.
            return tempSpecializationObj;
        }

        /// <summary>
        /// Delete a category by id, also deletes associated specializations.
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command object.
            using SqlCommand cmd = new SqlCommand("RemoveJobAdvertCategory")
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add category id parameter.
            cmd.Parameters.AddWithValue("CategoryId", id);

            // Execute command and catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Remove category specialization by id.
        /// </summary>
        /// <param name="id">Used to indentify the data to delete.</param>
        public void RemoveJobAdvertCategorySpecialization(int id)
        {
            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command object.
            using SqlCommand cmd = new SqlCommand("RemoveJobAdvertCategorySpecialization", id)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters
            cmd.Parameters.AddWithValue("SpecializationId", id);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update category.
        /// </summary>
        /// <param name="update">Used to update the current existing data with.</param>
        public void Update(JobAdvertCategory update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateJobAdvertCategory", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CategoryId", update.Id);
            cmd.Parameters.AddWithValue("NewCategoryName", update.Name);
            cmd.Parameters.AddWithValue("NewCategoryDescription", update.Description);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update category specialization.
        /// </summary>
        /// <param name="update">Used to update the existing data with new.</param>
        public void UpdateJobAdvertCategorySpecialization(JobAdvertCategorySpecialization update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateJobAdvertCategorySpecialization", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CategoryId", update.JobAdvertCategoryId);
            cmd.Parameters.AddWithValue("SpecializationId", update.Id);
            cmd.Parameters.AddWithValue("NewSpecializationName", update.Name);
            cmd.Parameters.AddWithValue("NewSpecializationDescription", update.Description);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }
    }
}
