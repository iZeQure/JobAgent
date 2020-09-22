using JobAgent.Data.Repository.Interface;
using JobAgent.Data.DataAccess;
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
            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateCategory", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("@name", create.Name);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute the command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Create a job advert category specialization.
        /// </summary>
        /// <param name="create">Used to specify the data for the specialization.</param>
        /// <param name="categoryId">Used to bind the specialization to a specific category.</param>
        public void CreateSpecialization(Specialization create, int categoryId)
        {
            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("CreateSpecialization", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Give the parameters some values.
            cmd.Parameters.AddWithValue("@id", categoryId);
            cmd.Parameters.AddWithValue("@name", create.Name);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Exectue the command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>A list of <see cref="Category"/></returns>
        public IEnumerable<Category> GetAll()
        {
            // Initialize a temporary list with categories.
            List<Category> tempCategories = new List<Category>();

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetAllCategories",
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

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

            // Prepare sql command object with information.
            using SqlCommand cmd = new SqlCommand("GetAllCategoriesWithSpecialization", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure,
                CommandTimeout = 5
            };

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

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
                    if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobSpecializationData.Name = reader.GetString("SpecializationName");
                    if (!DataReaderExtensions.IsDBNull(reader, "CategoryId")) jobSpecializationData.CategoryId = reader.GetInt32("CategoryId");

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
        public List<Specialization> GetAllSpecializations()
        {
            // Initialize a temporary list with specializations.
            List<Specialization> tempSpecializations = new List<Specialization>();

            // Prepare command with information to execute.
            using SqlCommand cmd = new SqlCommand("GetAllSpecializations", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

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
                        Id = reader.GetInt32("Id"),
                        Name = reader.GetString("Name"),
                        CategoryId = reader.GetInt32("CategoryId")
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

            // Prepare command object with information.
            using SqlCommand cmd = new SqlCommand("GetCategoryById", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any rows.
            if (reader.HasRows)
            {
                // Get the row specified by the id.
                tempCategory.Id = reader.GetInt32("Id");
                tempCategory.Name = reader.GetString("Name");
            }

            // return data
            return tempCategory;
        }

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="id">Used to identify the specialization to return.</param>
        /// <returns>A <see cref="Specialization"/>.</returns>
        public Specialization GetSpecializationById(int id)
        {
            // Initialize temporary data object.
            Specialization tempSpecializationObj = new Specialization();

            // Prepare command with information.
            using SqlCommand cmd = new SqlCommand("GetSpecializationById", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add specialization to get.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for rows
            if (reader.HasRows)
            {
                // Read the data
                while (reader.Read())
                {
                    tempSpecializationObj.Id = reader.GetInt32("Id");
                    tempSpecializationObj.Name = reader.GetString("Name");
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
            // Prepare command object.
            using SqlCommand cmd = new SqlCommand("RemoveCategory")
            {
                CommandType = CommandType.StoredProcedure
            };

            // Add category id parameter.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Remove category specialization by id.
        /// </summary>
        /// <param name="id">Used to indentify the data to delete.</param>
        public void RemoveSpecialization(int id)
        {
            // Prepare command object.
            using SqlCommand cmd = new SqlCommand("RemoveSpecialization", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update category.
        /// </summary>
        /// <param name="update">Used to update the current existing data with.</param>
        public void Update(Category update)
        {
            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCategory", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Id);
            cmd.Parameters.AddWithValue("@name", update.Name);

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update category specialization.
        /// </summary>
        /// <param name="update">Used to update the existing data with new.</param>
        public void UpdateSpecialization(Specialization update)
        {
            // Prepare command obj.
            using SqlCommand cmd = new SqlCommand("UpdateSpecialization", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@categoryId", update.CategoryId);
            cmd.Parameters.AddWithValue("@specializationId", update.Id);
            cmd.Parameters.AddWithValue("@name", update.Name);

            // Open connection to the database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }
    }
}