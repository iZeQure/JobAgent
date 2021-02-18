using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using DataAccess.Repositories.Interfaces;
using Pocos;
using DataAccess.SqlAccess;
using DataAccess.Repositories.Base;
using System;
using DataAccess.SqlAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public CategoryRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create a job advert category.
        /// </summary>
        /// <param name="create">Used to specify the data for the category.</param>
        public async Task Create(Category create)
        {
            try
            {
                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand("CreateCategory", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Give the parameters some values.
                cmd.Parameters.AddWithValue("@name", create.Name);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute the command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task Create(Specialization create)
        {
            try
            {
                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand("CreateSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Give the parameters some values.
                cmd.Parameters.AddWithValue("@id", create.CategoryId);
                cmd.Parameters.AddWithValue("@name", create.Name);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Exectue the command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Create a job advert category specialization.
        /// </summary>
        /// <param name="create">Used to specify the data for the specialization.</param>
        /// <param name="categoryId">Used to bind the specialization to a specific category.</param>
        public async Task CreateSpecialization(Specialization create, int categoryId)
        {
            try
            {
                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand("CreateSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Give the parameters some values.
                cmd.Parameters.AddWithValue("@id", categoryId);
                cmd.Parameters.AddWithValue("@name", create.Name);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Exectue the command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns>A list of <see cref="Category"/></returns>
        public Task<IEnumerable<Category>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                // Initialize a temporary list with categories.
                List<Category> tempCategories = new List<Category>();

                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetAllCategories",
                    Connection = _databaseAccess.GetConnection()
                };

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

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
                            Identifier = reader.GetInt32(0),
                            Name = reader.GetString(1)
                        });
                    }
                }

                // return data.
                return tempCategories;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all categories with associated specializations.
        /// </summary>
        /// <returns>A list of <see cref="Category" with a list of <see cref="Category.CategorySpecializations"/>/></returns>
        public async Task<IEnumerable<Category>> GetAllCategoriesWithSpecializations()
        {
            try
            {
                // Initialize a temporary list with categories.
                List<Category> tempCategories = new List<Category>();

                // Prepare sql command object with information.
                using SqlCommand cmd = new SqlCommand("GetAllCategoriesWithSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandTimeout = 5
                };

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize a data reader.
                //using SqlDataReader reader = await cmd.ExecuteReaderAsync();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
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
                            jobCategoryData.Identifier = reader.GetInt32("Id");
                            jobCategoryData.Name = reader.GetString("CategoryName");

                            // Specialization Data
                            if (!DataReaderExtensions.IsDBNull(reader, "SpecId")) jobSpecializationData.Identifier = reader.GetInt32("SpecId");
                            if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobSpecializationData.Name = reader.GetString("SpecializationName");
                            if (!DataReaderExtensions.IsDBNull(reader, "CategoryId")) jobSpecializationData.CategoryId = reader.GetInt32("CategoryId");

                            // Check if job category has a matching specialization.
                            if (jobCategoryData.Identifier == jobSpecializationData.CategoryId)
                            {
                                // Check if the category exists in the temp list.
                                if (tempCategories.Any(data => data.Identifier == jobCategoryData.Identifier))
                                {
                                    // Loop through temp list.
                                    foreach (var cat in tempCategories)
                                    {
                                        // Check if any specialization matches any categories in the temp list.
                                        if (cat.Identifier == jobSpecializationData.CategoryId)
                                        {
                                            cat.Specializations.Add(
                                                new Specialization()
                                                {
                                                    Identifier = jobSpecializationData.Identifier,
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
                                            Identifier = jobCategoryData.Identifier,
                                            Name = jobCategoryData.Name,
                                            Specializations = new List<Specialization>()
                                             {
                                         new Specialization()
                                         {
                                             Identifier = jobSpecializationData.Identifier,
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
                                        Identifier = jobCategoryData.Identifier,
                                        Name = jobCategoryData.Name,
                                        Specializations = null
                                    });
                            }
                        }
                    }
                }

                // return data.
                return tempCategories;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all category specializations.
        /// </summary>
        /// <returns>A list of <see cref="Specialization"/>.</returns>
        public async Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            try
            {
                // Initialize a temporary list with specializations.
                List<Specialization> tempSpecializations = new List<Specialization>();

                // Prepare command with information to execute.
                using SqlCommand cmd = new SqlCommand("GetAllSpecializations", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

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
                            Identifier = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            CategoryId = reader.GetInt32("CategoryId")
                        });
                    }
                }

                // Return data
                return tempSpecializations;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get category by id.
        /// </summary>
        /// <param name="id">Used to indicate which category to return.</param>
        /// <returns>A <see cref="Category"/>.</returns>
        public async Task<Category> GetById(int id)
        {
            try
            {
                // Initalize temporary category object.
                Category tempCategory = new Category();

                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand("GetCategoryById", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check if any rows.
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        // Get the row specified by the id.
                        tempCategory.Identifier = reader.GetInt32("Id");
                        tempCategory.Name = reader.GetString("Name");
                    }
                }

                // return data
                return tempCategory;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get category specialization by id.
        /// </summary>
        /// <param name="id">Used to identify the specialization to return.</param>
        /// <returns>A <see cref="Specialization"/>.</returns>
        public async Task<Specialization> GetSpecializationById(int id)
        {
            try
            {
                // Initialize temporary data object.
                Specialization tempSpecializationObj = new Specialization();

                // Prepare command with information.
                using SqlCommand cmd = new SqlCommand("GetSpecializationById", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add specialization to get.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check for rows
                if (reader.HasRows)
                {
                    // Read the data
                    while (reader.Read())
                    {
                        tempSpecializationObj.Identifier = reader.GetInt32("Id");
                        tempSpecializationObj.Name = reader.GetString("Name");
                    }
                }

                // return data obj.
                return tempSpecializationObj;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Delete a category by id, also deletes associated specializations.
        /// </summary>
        /// <param name="id"></param>
        public async Task Remove(int id)
        {
            try
            {
                // Prepare command object.
                using SqlCommand cmd = new SqlCommand("RemoveCategory")
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = _databaseAccess.GetConnection()
                };

                // Add category id parameter.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Remove category specialization by id.
        /// </summary>
        /// <param name="id">Used to indentify the data to delete.</param>
        public async Task RemoveSpecialization(int id)
        {
            try
            {
                // Prepare command object.
                using SqlCommand cmd = new SqlCommand("RemoveSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Update category.
        /// </summary>
        /// <param name="update">Used to update the current existing data with.</param>
        public async Task Update(Category update)
        {
            try
            {
                // Prepare command obj.
                using SqlCommand cmd = new SqlCommand("UpdateCategory", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier);
                cmd.Parameters.AddWithValue("@name", update.Name);

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task Update(Specialization update)
        {
            try
            {
                // Prepare command obj.
                using SqlCommand cmd = new SqlCommand("UpdateSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@categoryId", update.CategoryId);
                cmd.Parameters.AddWithValue("@specializationId", update.Identifier);
                cmd.Parameters.AddWithValue("@name", update.Name);

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Update category specialization.
        /// </summary>
        /// <param name="update">Used to update the existing data with new.</param>
        public async Task UpdateSpecialization(Specialization update)
        {
            try
            {
                // Prepare command obj.
                using SqlCommand cmd = new SqlCommand("UpdateSpecialization", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@categoryId", update.CategoryId);
                cmd.Parameters.AddWithValue("@specializationId", update.Identifier);
                cmd.Parameters.AddWithValue("@name", update.Name);

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        async Task<IEnumerable<Specialization>> IRepository<Specialization>.GetAll()
        {
            try
            {
                // Initialize a temporary list with specializations.
                List<Specialization> tempSpecializations = new List<Specialization>();

                // Prepare command with information to execute.
                using SqlCommand cmd = new SqlCommand("GetAllSpecializations", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Open connection to the database.
                await _databaseAccess.OpenConnectionAsync();

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
                            Identifier = reader.GetInt32("Id"),
                            Name = reader.GetString("Name"),
                            CategoryId = reader.GetInt32("CategoryId")
                        });
                    }
                }

                // Return data
                return tempSpecializations;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        async Task<Specialization> IRepository<Specialization>.GetById(int id)
        {
            try
            {
                // Initialize temporary data object.
                Specialization tempSpecializationObj = new Specialization();

                // Prepare command with information.
                using SqlCommand cmd = new SqlCommand("GetSpecializationById", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Add specialization to get.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check for rows
                if (reader.HasRows)
                {
                    // Read the data
                    while (reader.Read())
                    {
                        tempSpecializationObj.Identifier = reader.GetInt32("Id");
                        tempSpecializationObj.Name = reader.GetString("Name");
                    }
                }

                // return data obj.
                return tempSpecializationObj;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        async Task<Category> IRepository<Category>.GetById(int id)
        {
            try
            {
                // Initalize temporary category object.
                Category tempCategory = new Category();

                // Prepare command object with information.
                using SqlCommand cmd = new SqlCommand("GetCategoryById", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check if any rows.
                if (reader.HasRows)
                {
                    // Get the row specified by the id.
                    tempCategory.Identifier = reader.GetInt32("Id");
                    tempCategory.Name = reader.GetString("Name");
                }

                // return data
                return tempCategory;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}