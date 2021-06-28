using ObjectLibrary.Logging;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public LogRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a Log
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(Log createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spCreateLog";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@logSeverityId", createEntity.LogSeverity),
                    new SqlParameter("@createdDateTime", createEntity.CreatedDateTime),
                    new SqlParameter("@createdBy", createEntity.CreatedBy),
                    new SqlParameter("@action", createEntity.Action),
                    new SqlParameter("@message", createEntity.Message)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a Log
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Log deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spDeleteLog";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a collection of all Logs
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Log>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetLogs";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<Log> tempLogs = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        tempLogs.Add(new Log(
                            reader.GetInt32(0),
                            (LogSeverity)reader.GetInt32(1),
                            reader.GetDateTime(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5)
                            ));
                    }

                    return await Task.FromResult(tempLogs);
                }

                return await Task.FromResult(Enumerable.Empty<Log>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific Log by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Log> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetLogById";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Log log = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        log = new Log(
                             reader.GetInt32(0),
                            (LogSeverity)reader.GetInt32(1),
                            reader.GetDateTime(2),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetString(5));
                    }
                    return log;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a Log
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Log updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spUpdateLog";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@logSeverityId", updateEntity.LogSeverity),
                    new SqlParameter("@createdDateTime", updateEntity.CreatedDateTime),
                    new SqlParameter("@createdBy", updateEntity.CreatedBy),
                    new SqlParameter("@action", updateEntity.Action),
                    new SqlParameter("@message", updateEntity.Message)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
