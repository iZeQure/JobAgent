using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
using JobAgent.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        public void Create(JobAdvert create)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobAdvert> GetAll()
        {
            // Initialzie temporay job advet collection.
            List<JobAdvert> tempJobAdverts = new List<JobAdvert>();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand("GetAllJobAdverts", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

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
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        Email = reader.GetString(2),
                        PhoneNumber = reader.GetString(3),
                        JobDescription = reader.GetString(4),
                        JobLocation = reader.GetString(5),
                        JobRegisteredDate = reader.GetDateTime(6),
                        DeadlineDate = reader.GetDateTime(7),
                        CompanyCVR = new Company()
                        {
                            Id = reader.GetInt32(8)
                        },
                        JobAdvertCategoryId = new JobAdvertCategory()
                        {
                            Id = reader.GetInt32(9)
                        },
                        JobAdvertCategorySpecializationId = new JobAdvertCategorySpecialization()
                        {
                            Id = reader.GetInt32(10)
                        }
                    });
                }
            }

            // Return data.
            return tempJobAdverts;
        }

        public JobAdvert GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobVacanciesAdminModel> GetJobAdvertsForAdmins()
        {
            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllJobAdvertsForAdmins",
                CommandType = CommandType.StoredProcedure,
                Connection = Database.Instance.SqlConnection
            };

            // Initialize data reader.
            using SqlDataReader r = c.ExecuteReader();

            // Create temp list with job adverts.
            List<JobVacanciesAdminModel> dataList = new List<JobVacanciesAdminModel>();

            // Temp objects.
            JobAdvertCategorySpecialization specialization = new JobAdvertCategorySpecialization();

            // Check for rows.
            if (r.HasRows)
            {
                // Read data.
                while (r.Read())
                {
                    if (!DataReaderExtensions.IsDBNull(r, "Specialization")) specialization.Name = r.GetString("Specialization");
                    else specialization.Name = string.Empty;

                    dataList.Add(new JobVacanciesAdminModel()
                    {
                        JobAdvert = new JobAdvert()
                        {
                            Id = r.GetInt32("JobId"),
                            Title = r.GetString("Title"),
                            JobRegisteredDate = r.GetDateTime("JobRegisteredDate"),
                            DeadlineDate = r.GetDateTime("DeadlineDate")
                        },
                        Category = new JobAdvertCategory()
                        {
                            Name = r.GetString("Category")
                        },
                        Specialization = new JobAdvertCategorySpecialization()
                        {
                            Name = specialization.Name
                        },
                        Company = new Company()
                        {
                            Name = r.GetString("Name")
                        }
                    });
                }
            }

            return dataList;
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(JobAdvert update)
        {
            throw new NotImplementedException();
        }
    }
}
