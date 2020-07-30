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
        public void Create(JobAdvertCategory create)
        {
            throw new NotImplementedException();
        }

        public void CreateJobAdvertCategorySpecialization(JobAdvertCategorySpecialization create, int categoryId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JobAdvertCategory> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<JobAdvertCategory> GetAllJobAdvertCategoriesWithSpecializations()
        {
            List<JobAdvertCategory> tempJobAdvertCategories = new List<JobAdvertCategory>();

            using (Database.Instance.SqlConnection)
            {
                Database.Instance.OpenConnection();

                using SqlCommand cmd = new SqlCommand("GetAllJobAdvertCategoriesWithSpecialization", Database.Instance.SqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                using SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read())
                    {
                        var jobAdvertCategory = new JobAdvertCategory();
                        var jobAdvertCategorySpecialization = new JobAdvertCategorySpecialization();

                        if (!DataReaderExtensions.IsDBNull(reader, "Id")) jobAdvertCategory.Id = reader.GetInt32(0);
                        if (!DataReaderExtensions.IsDBNull(reader, "CategoryName")) jobAdvertCategory.Name = reader.GetString(1);
                        if (!DataReaderExtensions.IsDBNull(reader, "CategoryDescription")) jobAdvertCategory.Description = reader.GetString(2);
                        if (!DataReaderExtensions.IsDBNull(reader, "JobAdvertCategoryId")) jobAdvertCategorySpecialization.Id = reader.GetInt32(3);
                        if (!DataReaderExtensions.IsDBNull(reader, "SpecializationName")) jobAdvertCategorySpecialization.Name = reader.GetString(4);
                        if (!DataReaderExtensions.IsDBNull(reader, "SpecializationDescription")) jobAdvertCategorySpecialization.Description = reader.GetString(5);

                        if (jobAdvertCategory.Id == jobAdvertCategorySpecialization.Id)
                        {
                            if (tempJobAdvertCategories.Any(id => id.Id == jobAdvertCategory.Id))
                            {
                                foreach (var category in tempJobAdvertCategories)
                                {
                                    if (category.Id == jobAdvertCategorySpecialization.Id)
                                    {
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

                reader.Close();

                return tempJobAdvertCategories;
            }
        }

        public List<JobAdvertCategorySpecialization> GetAllJobAdvertCategorySpecializations()
        {
            throw new NotImplementedException();
        }

        public JobAdvertCategory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public JobAdvertCategorySpecialization GetJobAdvertCategorySpecializationById(int categoryId, int specializationId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveJobAdvertCategorySpecialization(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(JobAdvertCategory update)
        {
            throw new NotImplementedException();
        }

        public void UpdateJobAdvertCategorySpecialization(JobAdvertCategorySpecialization update)
        {
            throw new NotImplementedException();
        }
    }
}
