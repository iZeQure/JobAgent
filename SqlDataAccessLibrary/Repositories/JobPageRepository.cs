using ObjectLibrary.Common;
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
    public class JobPageRepository : IJobPageRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public JobPageRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a new JobPage
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(JobPage createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spCreateJobPage";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@companyId", createEntity.Company.Id),
                    new SqlParameter("@jobPageUrl", createEntity.URL)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a JobPage
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(JobPage deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spDeleteJobPage";
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
        /// Returns a collection with all JobPages
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobPage>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetJobPages";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<JobPage> jobPages = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        jobPages.Add(new JobPage(
                            reader.GetInt32(0),
                            new Company(
                                reader.GetInt32(1),
                                0,
                                string.Empty,
                                string.Empty),
                            reader.GetString(2)));
                    }

                    return await Task.FromResult(jobPages);
                }

                return await Task.FromResult(Enumerable.Empty<JobPage>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific JobPage by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<JobPage> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetJobPageById";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    JobPage job = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        job = new JobPage(
                            reader.GetInt32(0),
                            new Company(reader.GetInt32(1), 0, string.Empty, string.Empty),
                            reader.GetString(2));
                    }
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a JobPage
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(JobPage updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spUpdateJobPage";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@companyId", updateEntity.Company.Id),
                    new SqlParameter("@jobPageUrl", updateEntity.URL)
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
