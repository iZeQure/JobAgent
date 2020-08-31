using JobAgent.Data.Repository.Interface;
using JobAgent.Data.DB;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        /// <summary>
        /// Create a job advert category.
        /// </summary>
        /// <param name="create">Used to specify the data for the category.</param>
        public void Create(Category create)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateCategory", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("CategoryName", create.Name);

            // Execute the command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a job advert category specialization.
        /// </summary>
        /// <param name="create">Used to specify the data for the specialization.</param>
        /// <param name="categoryId">Used to bind the specialization to a specific category.</param>
        public void CreateCategorySpecialization(Specialization create, int categoryId)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateCategorySpecialization", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("CategoryID", categoryId);
            cmd.Parameters.AddWithValue("SpecializationName", create.Name);

            // Exectue the command, catch the return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>A list of <see cref="Category"/></returns>
        public IEnumerable<Category> GetAll()
        {
            // Initialize a temporary list with categories.
            List<Category> tempCategories = new List<Category>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetAllCategories",
                Connection = Database.Instance.SqlConnection
            };

            // Initialize Data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Temp
            Category temp = new Category();

            // Check if the reader has rows.
            if (reader.HasRows)
            {
                // Add data to the temp list while reading the data.
                while (reader.Read())
                {
                    tempCategories.Add(new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    });
                }
            }

            // return data.
            return tempCategories;
        }

        /// <summary>
        /// Get all categories with associated specializations.
        /// </summary>
        /// <returns>A list of <see cref="Category" with a list of <see cref="Category.CategorySpecializations"/>/></returns>
        public List<Category> GetAllCategoriesWithSpecializations()
        {
            // Initialize a temporary list with categories.
            List<Category> tempCategories = new List<Category>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare sql command object with information.
            using SqlCommand cmd = new SqlCommand("GetAllCategoriesWithSpecialization", Database.Instance.SqlConnection)
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
                    var Category = new Category();
                    var CategorySpecialization = new Specialization();

                    var jobCategoryData = new Category();
                    var jobSpecializationData = new Specialization();

                    // Test

                    // Category Data
                    jobCategoryData.Id = reader.GetInt32("Id");
                    jobCategoryData.Name = reader.GetString("CategoryName");

                    // Specialization Data
                    if (!DataReaderExtensions.IsDBNull(reader, "SpecId")) jobSpecializationData.Id = reader.GetInt32("SpecId");
                    if (!DataReaderExtensions.IsDBNull(reader, "JobAdvertCategoryId")) jobSpecializationData.CategoryId = reader.GetInt32("JobAdvertCategoryId");
                    if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobSpecializationData.Name = reader.GetString("SpecializationName");

                    // Check if job category has a matching specialization.
                    if (jobCategoryData.Id == jobSpecializationData.CategoryId)
                    {
                        // Check if the category exists in the temp list.
                        if (tempCategories.Any(data => data.Id == jobCategoryData.Id))
                        {
                            // Loop through temp list.
                            foreach (var cat in tempCategories)
                            {
                                // Check if any specialization matches any categories in the temp list.
                                if (cat.Id == jobSpecializationData.CategoryId)
                                {
                                    cat.Specializations.Add(
                                        new Specialization()
                                        {
                                            Id = jobSpecializationData.Id,
                                            Name = jobSpecializationData.Name,
                                            CategoryId = jobSpecializationData.CategoryId
                                        });
                                }
                            }
                        }
                        else
                        {
                            // Add category to the temp list, when it's non existent in the temp list.
                            tempCategories.Add(
                                new Category()
                                {
                                    Id = jobCategoryData.Id,
                                    Name = jobCategoryData.Name,
                                    Specializations = new List<Specialization>()
                                     {
                                         new Specialization()
                                         {
                                             Id = jobSpecializationData.Id,
                                             Name = jobSpecializationData.Name,
                                             CategoryId = jobSpecializationData.CategoryId
                                         }
                                     }
                                });
                        }
                    }
                    else
                    {
                        // Add category if non existent in the current temp list.
                        tempCategories.Add(
                            new Category()
                            {
                                Id = jobCategoryData.Id,
                                Name = jobCategoryData.Name,
                                Specializations = null
                            });
                    }


                    // Check if the values from the database contains null.
                    //if (!DataReaderExtensions.IsDBNull(reader, "Id")) jobAdvertCategory.Id = reader.GetInt32(0);
                    //if (!DataReaderExtensions.IsDBNull(reader, "CategoryName")) jobAdvertCategory.Name = reader.GetString(1);
                    //if (!DataReaderExtensions.IsDBNull(reader, "CategoryDescription")) jobAdvertCategory.Description = reader.GetString(2);
                    //if (!DataReaderExtensions.IsDBNull(reader, "JobAdvertCategoryId")) jobAdvertCategorySpecialization.JobAdvertCategoryId.Id = reader.GetInt32(3);
                    //if (!DataReaderExtensions.IsDBNull(reader, "SpecId")) jobAdvertCategorySpecialization.Id = reader.GetInt32(4);
                    //if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobAdvertCategorySpecialization.Name = reader.GetString(5);
                    //if (!DataReaderExtensions.IsDBNull(reader, "SpecializationDescription")) jobAdvertCategorySpecialization.Description = reader.GetString(6);

                    // If the Category ID Matches the specialization associated ID.
                    //if (jobAdvertCategory.Id == jobAdvertCategorySpecialization.Id)
                    //{
                    //    // Check if the category exists in the list.
                    //    if (tempJobAdvertCategories.Any(id => id.Id == jobAdvertCategory.Id))
                    //    {
                    //        // Loop through category list.
                    //        foreach (var category in tempJobAdvertCategories)
                    //        {
                    //            // Check if the specialization id matches any category id's in the list.
                    //            if (category.Id == jobAdvertCategorySpecialization.Id)
                    //            {
                    //                // Add specialization to the category with the associated list of specializations.
                    //                category.JobAdvertCategorySpecializations.Add(
                    //                new JobAdvertCategorySpecialization()
                    //                {
                    //                    Id = jobAdvertCategorySpecialization.Id,
                    //                    Name = jobAdvertCategorySpecialization.Name,
                    //                    Description = jobAdvertCategorySpecialization.Description,
                    //                    JobAdvertCategoryId = new JobAdvertCategory()
                    //                    {
                    //                        Id = jobAdvertCategorySpecialization.JobAdvertCategoryId.Id
                    //                    }
                    //                });
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        // Add Category with specialization if not exists in the current temporary list.
                    //        tempJobAdvertCategories.Add(new JobAdvertCategory()
                    //        {
                    //            Id = jobAdvertCategory.Id,
                    //            Name = jobAdvertCategory.Name,
                    //            Description = jobAdvertCategory.Description,
                    //            JobAdvertCategorySpecializations = new List<JobAdvertCategorySpecialization>()
                    //                {
                    //                    new JobAdvertCategorySpecialization()
                    //                    {
                    //                        Id = jobAdvertCategorySpecialization.Id,
                    //                        Name = jobAdvertCategorySpecialization.Name,
                    //                        Description = jobAdvertCategorySpecialization.Description
                    //                    }
                    //                }
                    //        });
                    //    }
                    //}
                    //else
                    //{
                    //    // Add new category if not exists.
                    //    tempJobAdvertCategories.Add(new JobAdvertCategory()
                    //    {
                    //        Id = jobAdvertCategory.Id,
                    //        Name = jobAdvertCategory.Name,
                    //        Description = jobAdvertCategory.Description,
                    //        JobAdvertCategorySpecializations = null
                    //    });
                    //}
                }
            }

            // Dispose the reader.
            reader.Close();

            // return data.
            return tempCategories;
        }

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>A list of <see cref="Specialization"/>.</returns>
        public List<Specialization> GetAllCategorySpecializations()
        {
            // Initialize a temporary list with specializations.
            List<Specialization> tempSpecializations = new List<Specialization>();

            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command with information to execute.
            using SqlCommand cmd = new SqlCommand("GetAllCategorySpecializations", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Temp
            Specialization temp = new Specialization();

            // Check if any rows.
            if (reader.HasRows)
            {
                // Add data to temporary list while reading.
                while (reader.Read())
                {
                    tempSpecializations.Add(new Specialization()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        CategoryId = reader.GetInt32(3)
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
        /// <returns>A <see cref="Category"/>.</returns>
        public Category GetById(int id)
        {
            // Initalize temporary category object.
            Category tempCategory = new Category();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("GetCategoryById", Database.Instance.SqlConnection)
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
            }

            // return data
            return tempCategory;
        }

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="id">Used to identify the specialization to return.</param>
        /// <returns>A <see cref="Specialization"/>.</returns>
        public Specialization GetCategorySpecializationById(int id)
        {
            // Initialize temporary data object.
            Specialization tempSpecializationObj = new Specialization();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command with information.
            using SqlCommand cmd = new SqlCommand("GetCategorySpecializationById", Database.Instance.SqlConnection)
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
            using SqlCommand cmd = new SqlCommand("RemoveCategory")
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
        public void RemoveCategorySpecialization(int id)
        {
            // Open connection to the database.
            Database.Instance.OpenConnection();

            // Prepare command object.
            using SqlCommand cmd = new SqlCommand("RemoveCategorySpecialization", Database.Instance.SqlConnection)
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
        public void Update(Category update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCategory", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CategoryId", update.Id);
            cmd.Parameters.AddWithValue("NewCategoryName", update.Name);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update category specialization.
        /// </summary>
        /// <param name="update">Used to update the existing data with new.</param>
        public void UpdateCategorySpecialization(Specialization update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCategorySpecialization", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CategoryId", update.CategoryId);
            cmd.Parameters.AddWithValue("SpecializationId", update.Id);
            cmd.Parameters.AddWithValue("NewSpecializationName", update.Name);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }
    }
}