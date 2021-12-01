using Dapper;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Entities.EntityMaps;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Entities.EntityMaps;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities.EntityMaps;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebCrawler.DataAccess
{
    public class DbCommunicator
    {
        private readonly string _connection;

        public DbCommunicator(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("Default");
        }

        public async Task<List<ICategory>> GetCategoriesAsync()
        {
            List<ICategory> categories = new();

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetCategories]";

                var queryResult = await conn.QueryAsync<CategoryInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        categories.Add(new Category()
                        {
                            Id = result.Id,
                            Name = result.Name
                        });
                    }
                }
            }
            return categories;
        }

        public async Task<List<ISpecialization>> GetSpecializationsAsync()
        {
            List<ISpecialization> specializations = new();

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetSpecializations]";

                var queryResults = await conn.QueryAsync<SpecializationInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResults is not null)
                {
                    foreach (var result in queryResults)
                    {
                        specializations.Add(new Specialization()
                        {
                            Id = result.Id,
                            CategoryId = result.CategoryId,
                            Name = result.Name
                        });
                    }
                }
            }

            return specializations;
        }

        public async Task<List<IDynamicSearchFilter>> GetDynamicSearchFiltersAsync()
        {
            List<IDynamicSearchFilter> DynamicSearchFilters = new();

            using (var conn = new SqlConnection(_connection))
            {
                string proc = "[JA.spGetDynamicFilterKeys]";

                var queryResult = await conn.QueryAsync<DynamicSearchFilterInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        DynamicSearchFilters.Add(new DynamicSearchFilter()
                        {
                            Id = result.DynamicSearchFilterId,
                            SpecializationId = result.SpecializationId,
                            CategoryId = result.CategoryId,
                            Key = result.DynamicSearchFilterKey
                        });
                    }
                }
            }
            return DynamicSearchFilters;
        }

        public async Task<List<IStaticSearchFilter>> GetStaticSearchFiltersAsync()
        {
            List<IStaticSearchFilter> staticSearchFilters = new();

            using (var conn = new SqlConnection(_connection))
            {
                string proc = "[JA.spGetStaticFilterKeys]";

                var queryResult = await conn.QueryAsync<StaticSearchFilterInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        staticSearchFilters.Add(new StaticSearchFilter()
                        {
                            Id = result.StaticSearchFilterId,
                            FilterType = new FilterType() { Id = result.FilterTypeId },
                            Key = result.StaticSearchFilterKey

                        });
                    }
                }
            }
            return staticSearchFilters;
        }

        public async Task<List<IJobPage>> GetJobPagesAsync()
        {
            List<IJobPage> jobPages = new();

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetJobPages]";

                var queryResult = await conn.QueryAsync<JobPageInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        jobPages.Add(new JobPage()
                        {
                            Id = result.Id,
                            CompanyId = result.CompanyId,
                            URL = result.URL
                        });
                    }
                }
            }
            return jobPages;
        }

        public async Task<List<IVacantJob>> GetVacantJobsAsync()
        {
            List<IVacantJob> vacantJobs = new();

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetVacantJobs]";

                var queryResult = await conn.QueryAsync<VacantJobInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        vacantJobs.Add(new VacantJob()
                        {
                            Id = result.Id,
                            CompanyId = result.CompanyId,
                            URL = result.URL
                        });
                    }
                }
            }
            return vacantJobs;
        }

        public async Task<List<IJobAdvert>> GetJobAdvertsAsync()
        {
            List<IJobAdvert> jobAdverts = new();

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetJobAdverts]";

                var queryResults = await conn.QueryAsync<JobAdvertInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResults is not null)
                {
                    foreach (var result in queryResults)
                    {
                        jobAdverts.Add(new JobAdvert()
                        {
                            Id = result.Id,
                            CategoryId = result.CategoryId,
                            SpecializationId = result.SpecializationId,
                            Title = result.Title,
                            Summary = result.Summary,
                            RegistrationDateTime = result.RegistrationDateTime
                        });
                    }
                }
            }
            return jobAdverts;
        }

        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            int entityId = 0;

            using (var conn = new SqlConnection(_connection))
            {
                string proc = "[JA.spCreateJobAdvert]";

                var values = new
                {
                    @vacantJobId = entity.Id,
                    @categoryId = entity.CategoryId,
                    @specializationId = entity.SpecializationId,
                    @jobAdvertTitle = entity.Title,
                    @jobAdvertSummary = entity.Summary,
                    @jobAdvertRegistrationDateTime = entity.RegistrationDateTime
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetJobAdvertByIdAsync(entityId);

            return null;
        }

        public async Task<IJobAdvert> GetJobAdvertByIdAsync(int id)
        {
            IJobAdvert jobAdvert = null;
            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetJobAdvertById]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<JobAdvertInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    jobAdvert = new JobAdvert()
                    {
                        Id = queryResult.Id,
                        CategoryId = queryResult.CategoryId,
                        SpecializationId = queryResult.SpecializationId,
                        Title = queryResult.Title,
                        Summary = queryResult.Summary,
                        RegistrationDateTime = queryResult.RegistrationDateTime
                    };

                }
            }
            return jobAdvert;
        }

        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            int entityId = 0;

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spUpdateJobAdvert]";
                var values = new
                {
                    @vacantJobId = entity.Id,
                    @specializationId = entity.SpecializationId,
                    @categoryId = entity.CategoryId,
                    @jobAdvertTitle = entity.Title,
                    @jobAdvertSummary = entity.Summary,
                    @jobAdvertRegistrationDateTime = entity.RegistrationDateTime
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetJobAdvertByIdAsync(entityId);

            return null;
        }

        public async Task<IVacantJob> CreateVacantJobAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spCreateVacantJob]";
                var values = new
                {
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetVacantJobByIdAsync(entityId);

            return null;
        }

        public async Task<IVacantJob> GetVacantJobByIdAsync(int id)
        {
            IVacantJob vacantJob = null;
            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spGetVacantJobById]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<VacantJobInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    vacantJob = new VacantJob()
                    {
                        Id = queryResult.Id,
                        CompanyId = queryResult.CompanyId,
                        URL = queryResult.URL
                    };
                }
            }
            return vacantJob;
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = new SqlConnection(_connection))
            {
                var proc = "[JA.spUpdateVacantJob]";
                var values = new
                {
                    @vacantJobId = entity.Id,
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetVacantJobByIdAsync(entityId);

            return null;
        }
    }
}
