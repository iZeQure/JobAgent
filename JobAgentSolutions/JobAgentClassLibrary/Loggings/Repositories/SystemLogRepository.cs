using Dapper;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings.Entities;
using JobAgentClassLibrary.Loggings.Entities.EntityMaps;
using JobAgentClassLibrary.Loggings.Factory;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Loggings.Repositories
{
    public class SystemLogRepository : ILoggingRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly LogEntityFactory _factory;

        public SystemLogRepository(ISqlDbManager sqlDbManager, LogEntityFactory factory)
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
                    @severityId = (int)entity.LogSeverity,
                    @currentTime = entity.CreatedDateTime,
                    @createdBy = entity.CreatedBy,
                    @action = entity.Action,
                    @message = entity.Message,
                    @logType = entity.LogType
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<ILog>> GetAllSystemLogsAsync()
        {
            List<ILog> logs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetDbLogs]";

                var queryResult = await conn.QueryAsync<LogInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ILog log = (SystemLog)_factory.CreateEntity(
                            nameof(SystemLog),
                            result.LogId,
                            result.LogSeverity,
                            result.LogMessage,
                            result.LogAction,
                            result.LogCreatedBy,
                            result.LogCreatedDateTime,
                            result.LogType);

                        logs.Add(log);
                    }
                }
            }

            return logs;
        }

        public async Task<List<ILog>> GetAllCrawlerLogsAsync()
        {
            List<ILog> logs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetCrawlerLogs]";

                var queryResult = await conn.QueryAsync<LogInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ILog log = (SystemLog)_factory.CreateEntity(
                            nameof(SystemLog),
                            result.LogId,
                            result.LogSeverity,
                            result.LogMessage,
                            result.LogAction,
                            result.LogCreatedBy,
                            result.LogCreatedDateTime,
                            result.LogType);

                        logs.Add(log);
                    }
                }
            }

            return logs;
        }

        public async Task<ILog> GetByIdAsync(int id)
        {
            ILog log = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetLogById]";
                var values = new
                {
                    @logId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<LogInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    log = (ILog)_factory.CreateEntity(
                           nameof(SystemLog),
                           queryResult.LogId,
                           queryResult.LogSeverity,
                           queryResult.LogMessage,
                           queryResult.LogAction,
                           queryResult.LogCreatedBy,
                           queryResult.LogCreatedDateTime,
                           queryResult.LogType);
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
                    @severityId = (int)entity.LogSeverity,
                    @createdDateTime = entity.CreatedDateTime,
                    @createdBy = entity.CreatedBy,
                    @action = entity.Action,
                    @message = entity.Message,
                    @logType = entity.LogType
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
