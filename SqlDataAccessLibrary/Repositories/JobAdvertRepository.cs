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
    public class JobAdvertRepository : IJobAdvertRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public JobAdvertRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a new JobAdvert
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(JobAdvert createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spCreateJobAdvert]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@categoryId", createEntity.Category.Id),
                    new SqlParameter("@specializationid", createEntity.Specialization.Id),
                    new SqlParameter("@jobAdvertTitle", createEntity.Title),
                    new SqlParameter("@jobAdvertSummary", createEntity.Summary),
                    new SqlParameter("@jobAdvertRegistrationDateTime", createEntity.RegistrationDateTime)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a JobAdvert
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(JobAdvert deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spDeleteJobAdvert]";
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
        /// Returns a collection with all JobAdverts
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<JobAdvert>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetJobAdverts]";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<JobAdvert> jobAdverts = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        VacantJob vacantjob = null;

                        jobAdverts.Add(new JobAdvert(
                            vacantjob,
                            new Category(reader.GetInt32(1), string.Empty),
                            new Specialization(reader.GetInt32(2), null, string.Empty),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetDateTime(5)));
                    }

                    return await Task.FromResult(jobAdverts);
                }

                return await Task.FromResult(Enumerable.Empty<JobAdvert>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<IEnumerable<JobAdvert>> GetAllUncategorized(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns a specific JobAdvert by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<JobAdvert> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetJobAdvertById]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    JobAdvert jobAdvert = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        VacantJob vacantjob = null;


                        jobAdvert = new JobAdvert(
                            vacantjob,
                            new Category(reader.GetInt32(1), string.Empty),
                            new Specialization(reader.GetInt32(2), null, string.Empty),
                            reader.GetString(3),
                            reader.GetString(4),
                            reader.GetDateTime(5));
                    }

                    return await Task.FromResult(jobAdvert);
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetJobAdvertCountByCategoryId(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetTotalJobAdvertCountByCategoryId]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countByCategory", SqlDbType = SqlDbType.Int };

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@categoryId", id),
                    outputParameter
                };

                await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                bool parsedOutput = int.TryParse(parameters[1].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return await Task.FromResult(count);
                }

                return await Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetJobAdvertCountBySpecializationId(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetTotalJobAdvertCountBySpecializationId]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countBySpecialization", SqlDbType = SqlDbType.Int };

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@specializationId", id),
                    outputParameter
                };

                await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                bool parsedOutput = int.TryParse(parameters[1].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return await Task.FromResult(count);
                }

                return await Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetJobAdvertCountByUncategorized(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetTotalJobAdvertCountByNonCategorized]";

                SqlParameter outputParameter = new() { Direction = ParameterDirection.Output, ParameterName = "@countByNonCategorized", SqlDbType = SqlDbType.Int };

                SqlParameter[] parameters = new[]
                {
                    outputParameter
                };

                await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                bool parsedOutput = int.TryParse(parameters[0].Value.ToString(), out int count);
                if (parsedOutput)
                {
                    return await Task.FromResult(count);
                }

                return await Task.FromResult(0);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a JobAdvert
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(JobAdvert updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spUpdateJobAdvert]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@categoryId", updateEntity.Category.Id),
                    new SqlParameter("@specializationid", updateEntity.Specialization.Id),
                    new SqlParameter("@jobAdvertTitle", updateEntity.Title),
                    new SqlParameter("@jobAdvertSummary", updateEntity.Summary),
                    new SqlParameter("@jobAdvertRegistrationDateTime", updateEntity.RegistrationDateTime)
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
