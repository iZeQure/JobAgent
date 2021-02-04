using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        public async void Create(JobAdvert create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

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
            await db.OpenConnectionAsync();

            await c.ExecuteNonQueryAsync();
        }

        public async Task<IEnumerable<JobAdvert>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            try
            {
                // Initialzie temporay job advet collection.
                List<JobAdvert> tempJobAdverts = new List<JobAdvert>();

                // Initialzie command obj.
                using SqlCommand cmd = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetAllJobAdverts",
                    Connection = db.GetConnection(),
                    CommandTimeout = 5
                };

                // Open connection to database.
                await db.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = await cmd.ExecuteReaderAsync();

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
            finally
            {
                db.CloseConnection();
            }
        }

        public async Task<JobAdvert> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetJobAdvertById",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection(),
                CommandTimeout = 10
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                JobAdvert tempData = new JobAdvert();

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

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
            finally
            {
                db.CloseConnection();
            }
        }

        public async Task<JobAdvert> GetJobAdvertDetailsForAdminsById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetJobAdvertDetailsForAdminsById",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@id", id);

            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader r = await c.ExecuteReaderAsync();

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

            db.CloseConnection();

            return tempJobAdvert;
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsForAdmins()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsForAdmins",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Open database connection.
            await db.OpenConnectionAsync();

            // Create temp list with job adverts.
            List<JobAdvert> dataList = new List<JobAdvert>();

            try
            {
                // Initialize data reader.
                using SqlDataReader r = await c.ExecuteReaderAsync();

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
            }
            finally
            {
                db.CloseConnection();
            }

            return dataList;
        }

        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Define id to remove.
            c.Parameters.AddWithValue("@id", id);

            try
            {
                await db.OpenConnectionAsync();

                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public async void Update(JobAdvert update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialzie command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

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

            // Open connection to db.
            await db.OpenConnectionAsync();

            // Update data.
            await c.ExecuteNonQueryAsync();

            // Close connection to db.
            db.CloseConnection();
        }

        public async Task<int> GetCountOfJobAdvertsByCategoryId(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int count = 0;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([CategoryId]) AS 'CategoryCount' FROM [JobAdvert] WHERE [CategoryId] = @id",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                db.CloseConnection();
            }

            return count;
        }

        public async Task<int> GetCountOfJobAdvertsBySpecializationId(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int count = 0;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([SpecializationId]) AS 'SpecialCount' FROM [JobAdvert] WHERE [SpecializationId] = @id",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                db.CloseConnection();
            }

            return count;
        }

        public async Task<int> GetCountOfJobAdvertsUncategorized()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int count = 0;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([CategoryId]) AS 'UncategorizedCount' FROM [JobAdvert] WHERE [CategoryId] = 0",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = db.GetConnection()
            };

            try
            {
                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                db.CloseConnection();
            }

            return count;
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedByCategoryId(int categoryId)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsSortedByCategoryId",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection(),
                CommandTimeout = 15
            };

            c.Parameters.AddWithValue("@categoryId", categoryId).SqlDbType = SqlDbType.Int;

            try
            {
                List<JobAdvert> data = new List<JobAdvert>();

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

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
            finally
            {
                db.CloseConnection();
            }
        }

        public async Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedBySpecializationId(int specializationId)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsSortedBySpecializationId",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection(),
                CommandTimeout = 15
            };

            c.Parameters.AddWithValue("@specialityId", specializationId).SqlDbType = SqlDbType.Int;

            try
            {
                List<JobAdvert> data = new List<JobAdvert>();

                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

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
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
