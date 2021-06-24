using ObjectLibrary.Versioning;
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
    public class ReleaseTypeRepository : IReleaseTypeRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public ReleaseTypeRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a ReleaseType
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ReleaseType createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateReleaseType];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", createEntity.Id),
                    new SqlParameter("@ProjectInformationId", createEntity.Name)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a ReleaseType
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(ReleaseType deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteReleaseType];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a collection of all ReleaseTypes
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ReleaseType>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetReleaseTypes];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<ReleaseType> releaseTypes = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        releaseTypes.Add(new ReleaseType(
                            reader.GetInt32(0),
                            reader.GetString(1)
                            ));
                    }

                    return await Task.FromResult(releaseTypes);
                }

                return await Task.FromResult(Enumerable.Empty<ReleaseType>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific ReleaseType by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<ReleaseType> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetReleaseTypeById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    ReleaseType releaseType = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        releaseType = new ReleaseType(
                            reader.GetInt32(0),
                            reader.GetString(1)
                            );
                    }
                    return releaseType;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a ReleaseType
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ReleaseType updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateReleaseType];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@Name", updateEntity.Name)
                    
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
