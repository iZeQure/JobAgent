using Dapper;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings.Entities;
using JobAgentClassLibrary.Loggings.Entities.EntityMaps;
using JobAgentClassLibrary.Loggings.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public class DbLogRepository : ILoggingRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly LogEntityFactory _factory;

        public DbLogRepository(ISqlDbManager sqlDbManager, LogEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }


        public async Task<ILog> CreateAsync(ILog entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var proc = "[JA.spCreateLog]";
                var values = new
                {
                    @severrityId = entity.LogSeverity,
                    @currentTime = entity.CreatedDateTime,
                    @createdBy = entity.CreatedBy,
                    @action = entity.Action,
                    @message = entity.Message
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<ILog>> GetAllAsync()
        {
            List<ILog> vacantJobs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetLogs]";

                var queryResult = await conn.QueryAsync<LogInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ILog log = (ILog)_factory.CreateEntity(
                            nameof(DbLog),
                            result.LogId,
                            result.LogSeverity,
                            result.LogMessage,
                            result.LogAction,
                            result.LogCreatedBy,
                            result.LogCreatedDateTime);

                        vacantJobs.Add(log);
                    }
                }
            }

            return vacantJobs;
        }

        public async Task<ILog> GetByIdAsync(int id)
        {
            ILog log = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetLogId]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<LogInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    log = (ILog)_factory.CreateEntity(
                           nameof(DbLog),
                           queryResult.LogId,
                           queryResult.LogSeverity,
                           queryResult.LogMessage,
                           queryResult.LogAction,
                           queryResult.LogCreatedBy,
                           queryResult.LogCreatedDateTime);
                }

            }

            return log;
        }

        public async Task<bool> RemoveAsync(ILog entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveLog]";
                var values = new
                {
                    @logId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<ILog> UpdateAsync(ILog entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateLog]";
                var values = new
                {
                    @logId = entity.Id,
                    @severityId = entity.LogSeverity,
                    @createdDateTime = entity.CreatedDateTime,
                    @createdBy = entity.CreatedBy,
                    @action = entity.Action,
                    @message = entity.Message
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
