using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using JobAgent.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        public void Create(JobAdvert create)
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
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
            c.Parameters.AddWithValue("@companyId", create.Company.Id);
            c.Parameters.AddWithValue("@categoryId", create.Category.Id);
            c.Parameters.AddWithValue("@specializationId", create.Specialization.Id);

            // Open database connection.
            SqlDataAccess.Instance.OpenConnection();

            c.ExecuteNonQuery();
        }

        public IEnumerable<JobAdvert> GetAll()
        {
            try
            {
                // Initialzie temporay job advet collection.
                List<JobAdvert> tempJobAdverts = new List<JobAdvert>();

                // Initialzie command obj.
                using SqlCommand cmd = new SqlCommand()
                {
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "GetAllJobAdverts",
                    Connection = SqlDataAccess.Instance.SqlConnection,
                    CommandTimeout = 5
                };

                // Open connection to database.
                SqlDataAccess.Instance.OpenConnection();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check for data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (reader.Read())
                    {
                        // Add data to collection.
                        tempJobAdverts.Add(new JobAdvert()
                        {
                            Id = reader.GetInt32("Id"),
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
                                Id = reader.GetInt32("CompanyId"),
                                Name = reader.GetString("CompanyName")
                            },
                            Category = new Category()
                            {
                                Id = reader.GetInt32("CategoryId"),
                                Name = !DataReaderExtensions.IsDBNull(reader, "CategoryName") ? reader.GetString("CategoryName") : "Ikke Kategoriseret"
                            },
                            Specialization = new Specialization()
                            {
                                Id = reader.GetInt32("SpecializationId"),
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
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public JobAdvert GetById(int id)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetJobAdvertById",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection,
                CommandTimeout = 10
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                JobAdvert tempData = new JobAdvert();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        tempData = new JobAdvert()
                        {
                            Id = r.GetInt32(0),
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
                                Id = r.GetInt32(9),
                                Name = r.GetString(10)
                            },
                            Category = new Category()
                            {
                                Id = r.GetInt32(11),
                                Name = !DataReaderExtensions.IsDBNull(r, "CategoryName") ? r.GetString(12) : string.Empty
                            },
                            Specialization = new Specialization()
                            {
                                Id = r.GetInt32(13),
                                Name = !DataReaderExtensions.IsDBNull(r, "SpecializationName") ? r.GetString(14) : string.Empty
                            }
                        };
                    }
                }

                return tempData;
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public Task<JobAdvert> GetJobAdvertDetailsForAdminsById(int id)
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetJobAdvertDetailsForAdminsById",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.AddWithValue("@id", id);

            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader r = c.ExecuteReader();

            // Initialize temporary obj.
            JobAdvert tempJobAdvert = null;

            Specialization specialization = new Specialization();

            // Check if the data reader has rows.
            if (r.HasRows)
            {
                // Read data.
                while (r.Read())
                {
                    tempJobAdvert = new JobAdvert()
                    {
                        Id = r.GetInt32("BaseId"),
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
                            Id = r.GetInt32("CompanyId"),
                            Name = r.GetString("CompanyName")
                        },
                        Category = new Category()
                        {
                            Id = r.GetInt32("CategoryId"),
                            Name = !DataReaderExtensions.IsDBNull(r, "CategoryName") ? r.GetString("CategoryName") : "Ikke Kategoriseret"
                        },
                        Specialization = new Specialization()
                        {
                            Id = r.GetInt32("SpecializationId"),
                            Name = !DataReaderExtensions.IsDBNull(r, "SpecializationName") ? r.GetString("SpecializationName") : string.Empty
                        }
                    };
                }
            }

            SqlDataAccess.Instance.CloseConnection();

            return Task.FromResult(tempJobAdvert);
        }

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsForAdmins()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsForAdmins",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Open database connection.
            SqlDataAccess.Instance.OpenConnection();

            // Create temp list with job adverts.
            List<JobAdvert> dataList = new List<JobAdvert>();

            try
            {
                // Initialize data reader.
                using SqlDataReader r = c.ExecuteReader();

                // Check for rows.
                if (r.HasRows)
                {
                    // Read data.
                    while (r.Read())
                    {
                        dataList.Add(
                            new JobAdvert()
                            {
                                Id = r.GetInt32("JobId"),
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
                SqlDataAccess.Instance.CloseConnection();
            }

            return Task.FromResult((IEnumerable<JobAdvert>)dataList);
        }

        public void Remove(int id)
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Define id to remove.
            c.Parameters.AddWithValue("@id", id);

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                c.ExecuteNonQuery();
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public void Update(JobAdvert update)
        {
            // Initialzie command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateJobAdvert",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Set parameters.
            c.Parameters.AddWithValue("@id", update.Id);
            c.Parameters.AddWithValue("@title", update.Title);
            c.Parameters.AddWithValue("@email", update.Email);
            c.Parameters.AddWithValue("@phoneNumber", update.PhoneNumber);
            c.Parameters.AddWithValue("@jobDescription", update.JobDescription);
            c.Parameters.AddWithValue("@jobLocation", update.JobLocation);
            c.Parameters.AddWithValue("@regDate", update.JobRegisteredDate);
            c.Parameters.AddWithValue("@deadlineDate", update.DeadlineDate);
            c.Parameters.AddWithValue("@sourceURL", update.SourceURL);
            c.Parameters.AddWithValue("@companyId", update.Company.Id);
            c.Parameters.AddWithValue("@categoryId", update.Category.Id);
            c.Parameters.AddWithValue("@specializationId", update.Specialization.Id);

            // Open connection to db.
            SqlDataAccess.Instance.OpenConnection();

            // Update data.
            c.ExecuteNonQuery();

            // Close connection to db.
            SqlDataAccess.Instance.CloseConnection();
        }

        public Task<int> GetCountOfJobAdvertsByCategoryId(int id)
        {
            int count = 0;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([CategoryId]) AS 'CategoryCount' FROM [JobAdvert] WHERE [CategoryId] = @id",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return Task.FromResult(count);
        }

        public Task<int> GetCountOfJobAdvertsBySpecializationId(int id)
        {
            int count = 0;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([SpecializationId]) AS 'SpecialCount' FROM [JobAdvert] WHERE [SpecializationId] = @id",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return Task.FromResult(count);
        }

        public Task<int> GetCountOfJobAdvertsUncategorized()
        {
            int count = 0;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = $"SELECT COUNT([CategoryId]) AS 'UncategorizedCount' FROM [JobAdvert] WHERE [CategoryId] = 0",
                CommandType = CommandType.Text,
                CommandTimeout = 15,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        count = r.GetInt32(0);
                    }
                }
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return Task.FromResult(count);
        }

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedByCategoryId(int categoryId)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsSortedByCategoryId",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection,
                CommandTimeout = 15
            };

            c.Parameters.AddWithValue("@categoryId", categoryId).SqlDbType = SqlDbType.Int;

            try
            {
                List<JobAdvert> data = new List<JobAdvert>();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        data.Add(
                            new JobAdvert()
                            {
                                Id = r.GetInt32(0),
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
                                    Id = r.GetInt32(9),
                                    Name = r.GetString(10),
                                    URL = r.GetString(11)
                                },
                                Category = new Category()
                                {
                                    Id = r.GetInt32(12),
                                    Name = r.GetString(13)
                                }
                            });
                    }
                }

                return Task.FromResult(data as IEnumerable<JobAdvert>);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public Task<IEnumerable<JobAdvert>> GetAllJobAdvertsSortedBySpecializationId(int specializationId)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsSortedBySpecializationId",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection,
                CommandTimeout = 15
            };

            c.Parameters.AddWithValue("@specialityId", specializationId).SqlDbType = SqlDbType.Int;

            try
            {
                List<JobAdvert> data = new List<JobAdvert>();

                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        data.Add(
                            new JobAdvert()
                            {
                                Id = r.GetInt32(0),
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
                                    Id = r.GetInt32(9),
                                    Name = r.GetString(10),
                                    URL = r.GetString(11)
                                },
                                Category = new Category()
                                {
                                    Id = r.GetInt32(12),
                                    Name = r.GetString(13)
                                },
                                Specialization = new Specialization()
                                {
                                    Id = r.GetInt32(14),
                                    Name = r.GetString(15)
                                }
                            });
                    }

                }
                return Task.FromResult(data as IEnumerable<JobAdvert>);
            }
            finally
            {                
                SqlDataAccess.Instance.CloseConnection();
            }
        }
    }
}
