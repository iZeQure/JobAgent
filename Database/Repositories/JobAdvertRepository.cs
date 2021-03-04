using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.SqlAccess.Interfaces;
using System;

namespace DataAccess.Repositories
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public JobAdvertRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        public async Task Create(JobAdvert create)
        {
            try
            {
                // Initialize command obj.
                using var c = _databaseAccess.GetCommand("CreateJobAdvert", CommandType.StoredProcedure);

                // Define data parameters.
                c.Parameters.AddWithValue("@title", create.Title);
                c.Parameters.AddWithValue("@email", create.Email);
                c.Parameters.AddWithValue("@phoneNumber", create.PhoneNumber);
                c.Parameters.AddWithValue("@jobDescription", create.JobDescription);
                c.Parameters.AddWithValue("@jobLocation", create.JobLocation);
                c.Parameters.AddWithValue("@regDate", create.JobRegisteredDate);
                c.Parameters.AddWithValue("@deadlineDate", create.DeadlineDate);
                c.Parameters.AddWithValue("@sourceURL", create.SourceURL);
                c.Parameters.AddWithValue("@companyId", create.Company.Identifier);
                c.Parameters.AddWithValue("@categoryId", create.Category.Identifier);
                c.Parameters.AddWithValue("@specializationId", create.Specialization.Identifier);

                // Open database connection.
                await _databaseAccess.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetAll()
        {
            // Initialzie temporay job advet collection.
            List<JobAdvert> tempJobAdverts = new List<JobAdvert>();

            // Initialzie command obj.
            using var cmd = _databaseAccess.GetCommand("GetAllJobAdverts", CommandType.StoredProcedure);

            try
            {
                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Check for data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (await reader.ReadAsync())
                    {
                        // Add data to collection.
                        tempJobAdverts.Add(new JobAdvert()
                        {
                            Identifier = reader.GetInt32("Id"),
                            Title = reader.GetString("Title"),
                            Email = reader.GetString("Email"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            JobDescription = reader.GetString("JobDescription"),
                            JobLocation = reader.GetString("JobLocation"),
                            JobRegisteredDate = reader.GetDateTime("JobRegisteredDate"),
                            DeadlineDate = reader.GetDateTime("DeadlineDate"),
                            SourceURL = reader.GetString("SourceURL"),
                            Company = new Company()
                            {
                                Identifier = reader.GetInt32("CompanyId"),
                                Name = reader.GetString("CompanyName")
                            },
                            Category = new Category()
                            {
                                Identifier = reader.GetInt32("CategoryId"),
                                Name = !DataReaderExtensions.IsDBNull(reader, "CategoryName") ? reader.GetString("CategoryName") : "Ikke Kategoriseret"
                            },
                            Specialization = new Specialization()
                            {
                                Identifier = reader.GetInt32("SpecializationId"),
                                Name = !DataReaderExtensions.IsDBNull(reader, "SpecializationName") ? reader.GetString("SpecializationName") : string.Empty
                            }
                        });
                    }
                }

                // Return data.
                return tempJobAdverts;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<JobAdvert> GetById(int id)
        {
            try
            {
                using var command = _databaseAccess.GetCommand("GetJobAdvertById", CommandType.StoredProcedure);

                command.Parameters.AddWithValue("@id", id);

                JobAdvert tempData = new JobAdvert();

                using SqlDataReader r = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        tempData = new JobAdvert()
                        {
                            Identifier = r.GetInt32(0),
                            Title = r.GetString(1),
                            Email = r.GetString(2),
                            PhoneNumber = r.GetString(3),
                            JobDescription = r.GetString(4),
                            JobLocation = r.GetString(5),
                            JobRegisteredDate = r.GetDateTime(6),
                            DeadlineDate = r.GetDateTime(7),
                            SourceURL = r.GetString(8),
                            Company = new Company()
                            {
                                Identifier = r.GetInt32(9),
                                Name = r.GetString(10)
                            },
                            Category = new Category()
                            {
                                Identifier = r.GetInt32(11),
                                Name = !DataReaderExtensions.IsDBNull(r, "CategoryName") ? r.GetString(12) : string.Empty
                            },
                            Specialization = new Specialization()
                            {
                                Identifier = r.GetInt32(13),
                                Name = !DataReaderExtensions.IsDBNull(r, "SpecializationName") ? r.GetString(14) : string.Empty
                            }
                        };
                    }
                }

                return tempData;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<JobAdvert> GetJobAdvertDetailsForAdminsById(int id)
        {
            try
            {
                // Initialize command obj.
                using var command = _databaseAccess.GetCommand("GetJobAdvertDetailsForAdminsById", CommandType.StoredProcedure);

                command.Parameters.AddWithValue("@id", id);

                // Initialize data reader.
                using SqlDataReader r = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                // Initialize temporary obj.
                JobAdvert tempJobAdvert = null;

                Specialization specialization = new Specialization();

                // Check if the data reader has rows.
                if (r.HasRows)
                {
                    // Read data.
                    while (await r.ReadAsync())
                    {
                        tempJobAdvert = new JobAdvert()
                        {
                            Identifier = r.GetInt32("BaseId"),
                            Title = r.GetString("Title"),
                            Email = r.GetString("Email"),
                            PhoneNumber = r.GetString("PhoneNumber"),
                            JobDescription = r.GetString("Desc"),
                            JobLocation = r.GetString("Loc"),
                            JobRegisteredDate = r.GetDateTime("RegDate"),
                            DeadlineDate = r.GetDateTime("DeadlineDate"),
                            SourceURL = r.GetString("SourceURL"),

                            Company = new Company()
                            {
                                Identifier = r.GetInt32("CompanyId"),
                                Name = r.GetString("CompanyName")
                            },
                            Category = new Category()
                            {
                                Identifier = r.GetInt32("CategoryId"),
                                Name = !DataReaderExtensions.IsDBNull(r, "CategoryName") ? r.GetString("CategoryName") : "Ikke Kategoriseret"
                            },
                            Specialization = new Specialization()
                            {
                                Identifier = r.GetInt32("SpecializationId"),
                                Name = !DataReaderExtensions.IsDBNull(r, "SpecializationName") ? r.GetString("SpecializationName") : string.Empty
                            }
                        };
                    }
                }

                return tempJobAdvert;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsForAdmins()
        {
            try
            {
                // Initialize command obj.
                using var command = _databaseAccess.GetCommand("GetAllJobAdvertsForAdmins", CommandType.StoredProcedure);

                // Create temp list with job adverts.
                List<JobAdvert> dataList = new List<JobAdvert>();

                // Initialize data reader.
                using SqlDataReader r = await command.ExecuteReaderAsync(CommandBehavior.SequentialAccess);

                // Check for rows.
                if (r.HasRows)
                {
                    // Read data.
                    while (await r.ReadAsync())
                    {
                        dataList.Add(
                            new JobAdvert()
                            {
                                Identifier = r.GetInt32("JobId"),
                                Title = r.GetString("Title"),
                                JobRegisteredDate = r.GetDateTime("JobRegisteredDate"),
                                DeadlineDate = r.GetDateTime("DeadlineDate"),

                                Category = new Category()
                                {
                                    Name = !DataReaderExtensions.IsDBNull(r, "Category") ? r.GetString("Category") : string.Empty
                                },
                                Specialization = new Specialization()
                                {
                                    Name = !DataReaderExtensions.IsDBNull(r, "Specialization") ? r.GetString("Specialization") : string.Empty
                                },
                                Company = new Company()
                                {
                                    Name = r.GetString("Name")
                                }
                            }
                        );
                    }
                }
                return dataList;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task Remove(int id)
        {
            try
            {
                // Initialize command obj.
                using var c = _databaseAccess.GetCommand("RemoveJobAdvert", CommandType.StoredProcedure);

                // Define id to remove.
                c.Parameters.AddWithValue("@id", id);

                await _databaseAccess.OpenConnectionAsync();

                await c.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task Update(JobAdvert update)
        {
            try
            {
                // Initialzie command obj.
                using var c = _databaseAccess.GetCommand("UpdateJobAdvert", CommandType.StoredProcedure);

                // Set parameters.
                c.Parameters.AddWithValue("@id", update.Identifier);
                c.Parameters.AddWithValue("@title", update.Title);
                c.Parameters.AddWithValue("@email", update.Email);
                c.Parameters.AddWithValue("@phoneNumber", update.PhoneNumber);
                c.Parameters.AddWithValue("@jobDescription", update.JobDescription);
                c.Parameters.AddWithValue("@jobLocation", update.JobLocation);
                c.Parameters.AddWithValue("@regDate", update.JobRegisteredDate);
                c.Parameters.AddWithValue("@deadlineDate", update.DeadlineDate);
                c.Parameters.AddWithValue("@sourceURL", update.SourceURL);
                c.Parameters.AddWithValue("@companyId", update.Company.Identifier);
                c.Parameters.AddWithValue("@categoryId", update.Category.Identifier);
                c.Parameters.AddWithValue("@specializationId", update.Specialization.Identifier);

                // Open connection to _databaseAccess.
                await _databaseAccess.OpenConnectionAsync();

                // Update data.
                await c.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<int> GetCountOfJobAdvertsByCategoryId(int id)
        {
            try
            {
                int count = 0;

                // Initialize command obj.
                using var c = _databaseAccess.GetCommand("SELECT COUNT([CategoryId]) AS 'CategoryCount' FROM [JobAdvert] WHERE [CategoryId] = @id", CommandType.Text);

                c.Parameters.AddWithValue("@id", id);

                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
                return count;
            }
            catch (Exception) { throw; }
        }

        public async Task<int> GetCountOfJobAdvertsBySpecializationId(int id)
        {
            try
            {
                int count = 0;

                // Initialize command obj.
                using var c = _databaseAccess.GetCommand("SELECT COUNT([SpecializationId]) AS 'SpecialCount' FROM [JobAdvert] WHERE [SpecializationId] = @id", CommandType.Text);

                c.Parameters.AddWithValue("@id", id);

                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
                return count;
            }
            catch (Exception) { throw; }
        }

        public async Task<int> GetCountOfJobAdvertsUncategorized()
        {
            try
            {
                int count = 0;

                using var c = _databaseAccess.GetCommand("SELECT COUNT([CategoryId]) AS 'UncategorizedCount' FROM [JobAdvert] WHERE [CategoryId] = 0", CommandType.Text);

                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
                return count;
            }
            catch (Exception) { throw; }
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedByCategoryId(int categoryId)
        {
            try
            {
                using var c = _databaseAccess.GetCommand("GetAllJobAdvertsSortedByCategoryId", CommandType.StoredProcedure);

                c.Parameters.AddWithValue("@categoryId", categoryId).SqlDbType = SqlDbType.Int;

                List<JobAdvert> data = new List<JobAdvert>();

                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        data.Add(
                            new JobAdvert()
                            {
                                Identifier = r.GetInt32(0),
                                Title = r.GetString(1),
                                Email = r.GetString(2),
                                PhoneNumber = r.GetString(3),
                                JobDescription = r.GetString(4),
                                JobLocation = r.GetString(5),
                                JobRegisteredDate = r.GetDateTime(6),
                                DeadlineDate = r.GetDateTime(7),
                                SourceURL = r.GetString(8),
                                Company = new Company()
                                {
                                    Identifier = r.GetInt32(9),
                                    Name = r.GetString(10),
                                    URL = r.GetString(11)
                                },
                                Category = new Category()
                                {
                                    Identifier = r.GetInt32(12),
                                    Name = r.GetString(13)
                                }
                            });
                    }
                }

                return data;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedBySpecializationId(int specializationId)
        {
            using var c = _databaseAccess.GetCommand("GetAllJobAdvertsSortedBySpecializationId", CommandType.StoredProcedure);

            try
            {
                c.Parameters.AddWithValue("@specialityId", specializationId).SqlDbType = SqlDbType.Int;

                List<JobAdvert> data = new List<JobAdvert>();

                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        data.Add(
                            new JobAdvert()
                            {
                                Identifier = r.GetInt32(0),
                                Title = r.GetString(1),
                                Email = r.GetString(2),
                                PhoneNumber = r.GetString(3),
                                JobDescription = r.GetString(4),
                                JobLocation = r.GetString(5),
                                JobRegisteredDate = r.GetDateTime(6),
                                DeadlineDate = r.GetDateTime(7),
                                SourceURL = r.GetString(8),
                                Company = new Company()
                                {
                                    Identifier = r.GetInt32(9),
                                    Name = r.GetString(10),
                                    URL = r.GetString(11)
                                },
                                Category = new Category()
                                {
                                    Identifier = r.GetInt32(12),
                                    Name = r.GetString(13)
                                },
                                Specialization = new Specialization()
                                {
                                    Identifier = r.GetInt32(14),
                                    Name = r.GetString(15)
                                }
                            });
                    }

                }
                return data;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetAllUncategorized()
        {
            // Change to procedure at some point in life.
            var commandText = @"
                SELECT 
	                j.*,
	                [Company].*,
	                [Category].[Id] AS CategoryID,
	                [Category].[Name] AS CategoryName,
	                [Specialization].[Id] AS SpecializationID,
	                [Specialization].[Name] AS SpecializationName
                FROM [JobAdvert] AS j
                INNER JOIN [Company] ON [Company].[Id] = j.[CompanyId]
                INNER JOIN [Category] ON [Category].[Id] = j.[CategoryId]
                INNER JOIN [Specialization] ON [Specialization].[Id] = j.[SpecializationId]
                WHERE j.[CategoryId] = 0";

            var uncategorizedList = new List<JobAdvert>();

            try
            {
                var connection = _databaseAccess.GetConnection();

                using (var command = _databaseAccess.GetCommand(commandText, CommandType.Text))
                {
                    using (var dataReader = await _databaseAccess.GetSqlDataReader())
                    {
                        if (!dataReader.HasRows)
                        {
                            return uncategorizedList;
                        }

                        while (await dataReader.ReadAsync())
                        {
                            uncategorizedList.Add(new JobAdvert
                            {
                                Identifier = dataReader.GetInt32(0),
                                Title = dataReader.GetString(1),
                                Email = dataReader.GetString(2),
                                PhoneNumber = dataReader.GetString(3),
                                JobDescription = dataReader.GetString(4),
                                JobLocation = dataReader.GetString(5),
                                JobRegisteredDate = dataReader.GetDateTime(6),
                                DeadlineDate = dataReader.GetDateTime(7),
                                SourceURL = dataReader.GetString(8),
                                Company = new Company
                                {
                                    Identifier = dataReader.GetInt32(12),
                                    CVR = dataReader.GetInt32(13),
                                    Name = dataReader.GetString(14),
                                    URL = dataReader.GetString(15)
                                },
                                Category = new Category
                                {
                                    Identifier = dataReader.GetInt32(16),
                                    Name = !DataReaderExtensions.IsDBNull(dataReader, "CategoryName") ? dataReader.GetString(17) : "Ikke Kategoriseret"
                                },
                                Specialization = new Specialization
                                {
                                    Identifier = dataReader.GetInt32(18),
                                    Name = !DataReaderExtensions.IsDBNull(dataReader, "SpecializationName") ? dataReader.GetString(19) : "Ikke Kategoriseret"
                                }
                            });
                        }
                    }
                }

                return uncategorizedList;
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}