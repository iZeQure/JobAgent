using Dapper;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Entities.EntityMaps;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts.Repositories
{
    public class JobAdvertRepository : IJobAdvertRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly JobAdvertEntityFactory _factory;

        public JobAdvertRepository(ISqlDbManager sqlDbManager, JobAdvertEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                string proc = "[JA.spCreateJobAdvert]";

                var values = new
                {
                    @vacantJobId = entity.Id,
                    @categoryId = entity.CategoryId,
                    @specializationId = entity.SpecializationId,
                    @jobAdvertTitlte = entity.Title,
                    @jobAdvertSummary = entity.Summary,
                    @jobAdvertRegistrationDateTime = entity.RegistrationDateTime
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }


        public async Task<List<IJobAdvert>> GetAllAsync()
        {
            List<IJobAdvert> jobAdverts = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var proc = "[JA.spGetJobAdverts]";

                var queryResults = await conn.QueryAsync<JobAdvertInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResults is not null)
                {
                    foreach (var result in queryResults)
                    {
                        IJobAdvert jobAdvert = (IJobAdvert)_factory.CreateEntity(nameof(JobAdvert),
                            result.Id,
                            result.CategoryId,
                            result.SpecializationId,
                            result.Title,
                            result.Summary,
                            result.RegistrationDateTime);

                        jobAdverts.Add(jobAdvert);
                    }
                }
            }
            return jobAdverts;
        }

        public Task<List<JobAdvert>> GetAllUncategorized()
        {
            throw new NotImplementedException();
        }


        public async Task<IJobAdvert> GetByIdAsync(int id)
        {
            IJobAdvert jobAdvert = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var proc = "[JA.spGetJobAdvertById]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<JobAdvertInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    jobAdvert = (IJobAdvert)_factory.CreateEntity(nameof(JobAdvert),
                        queryResult.Id,
                        queryResult.CategoryId,
                        queryResult.SpecializationId,
                        queryResult.Title,
                        queryResult.Summary,
                        queryResult.RegistrationDateTime);
                }
            }
            return jobAdvert;
        }


        public async Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            int count = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Complex))
            {
                var proc = "[JA.spGetTotalJobAdvertCountByCategoryId]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@categoryId", id);
                dynamicValues.Add("@countByCategory", SqlDbType.Int, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                count = dynamicValues.Get<int>("@countByCategory");
            }

            return count;
        }


        public async Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            int count = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Complex))
            {
                var proc = "[JA.spGetTotalJobAdvertCountBySpecializationId]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@specializationId", id);
                dynamicValues.Add("@countByCategory", SqlDbType.Int, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                count = dynamicValues.Get<int>("@countByCategory");
            }

            return count;
        }


        public async Task<int> GetJobAdvertCountByUncategorized()
        {
            int count = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Complex))
            {
                var proc = "[JA.spGetTotalJobAdvertCountByNonCategorized]";
                var dynamicValues = new DynamicParameters();

                dynamicValues.Add("@countByCategory", SqlDbType.Int, direction: ParameterDirection.Output);

                await conn.QueryAsync(proc, dynamicValues, commandType: CommandType.StoredProcedure);

                count = dynamicValues.Get<int>("@countByCategory");
            }

            return count;
        }


        public async Task<bool> RemoveAsync(IJobAdvert entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var proc = "[JA.spRemoveJobAdvert]";
                var values = new
                {
                    @vacantJobId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }


        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
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

            if (entityId >= 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
