using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
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
