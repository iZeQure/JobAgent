using Dapper;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Entities.EntityMaps;
using JobAgentClassLibrary.Common.JobPages.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages.Repositories
{
    public class JobPageRepository : IJobPageRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly JobPageEntityFactory _factory;

        public JobPageRepository(ISqlDbManager sqlDbManager, JobPageEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<IJobPage> CreateAsync(IJobPage entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateJobPage]";
                
                var values = new
                {
                    @companyId = entity.CompanyId,
                    @jobPageUrl = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<IJobPage>> GetAllAsync()
        {
            List<IJobPage> jobPages = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetJobPages]";

                var queryResult = await conn.QueryAsync<JobPageInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IJobPage jobPage = (IJobPage)_factory.CreateEntity(nameof(JobPage), result.Id, result.CompanyId, result.URL);

                        jobPages.Add(jobPage);
                    }
                }
            }

            return jobPages;
        }

        public async Task<IJobPage> GetByIdAsync(int id)
        {
            IJobPage jobPage = null;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetJobPageById]";
                var values = new
                {
                    @jobPageId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<JobPageInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    jobPage = (IJobPage)_factory.CreateEntity(
                                nameof(JobPage),
                                queryResult.Id, queryResult.CompanyId, queryResult.URL);
                }
            }

            return jobPage;
        }

        public async Task<bool> RemoveAsync(IJobPage entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveJobPage]";
                var values = new
                {
                    @jobPageId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateJobPage]";
                var values = new
                {
                    @jobPageId = entity.Id,
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
