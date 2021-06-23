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
    public class VacantJobRepository : IVacantJobRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public VacantJobRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates new VacantJob
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(VacantJob createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateVacantJob];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@companyId", createEntity.Company.Id),
                    new SqlParameter("@vacantJobUrl", createEntity.URL)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a VacantJob
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(VacantJob deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteVacantJob];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@vacantJobId", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a collection of all VacantJobs
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VacantJob>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetVacantJobs];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<VacantJob> vacantJobs = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        vacantJobs.Add(new VacantJob(
                            reader.GetInt32(0),
                            new Company(
                                reader.GetInt32(1),
                                0,
                                string.Empty,
                                string.Empty),
                            reader.GetString(2)));
                    }

                    return await Task.FromResult(vacantJobs);
                }

                return await Task.FromResult(Enumerable.Empty<VacantJob>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific VacantJob by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<VacantJob> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetVacantJobById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@vacantJobId", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    VacantJob job = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        job = new VacantJob(
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
        /// Updates a VacantJob
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(VacantJob updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateVacantJob];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@vacantJobId", updateEntity.Id),
                    new SqlParameter("@companyId", updateEntity.Company.Id),
                    new SqlParameter("@vacantJobUrl", updateEntity.URL)
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
