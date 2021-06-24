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
    public class VersionControlRepository : IVersionControlRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public VersionControlRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a VersionControl
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(VersionControl createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateVersionControl];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", createEntity.Id),
                    new SqlParameter("@ProjectInformationId", createEntity.ProjectInformation.Id),
                    new SqlParameter("@ReleaseTypeId", createEntity.ReleaseType.Id),
                    new SqlParameter("@CommitId", createEntity.CommitId),
                    new SqlParameter("@Major", createEntity.Major),
                    new SqlParameter("@Minor", createEntity.Minor),
                    new SqlParameter("@Path", createEntity.Patch),
                    new SqlParameter("@ReleaseDateTime", createEntity.ReleaseDateTime)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a VersionControl
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(VersionControl deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteVersionControl];";
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
        /// Returns a collection of all VersionControls
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VersionControl>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetVacantJobs];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<VersionControl> versionControls = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        versionControls.Add(new VersionControl(
                            reader.GetInt32(0),
                            new ProjectInformation(reader.GetInt32(1), "", DateTime.Now),
                            new ReleaseType(reader.GetInt32(2), ""),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6),
                            reader.GetDateTime(7)
                            ));
                    }

                    return await Task.FromResult(versionControls);
                }

                return await Task.FromResult(Enumerable.Empty<VersionControl>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific VersionControl by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<VersionControl> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetVersionControlById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    VersionControl versionControl = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        versionControl = new VersionControl(
                            reader.GetInt32(0),
                            new ProjectInformation(reader.GetInt32(1), "", DateTime.Now),
                            new ReleaseType(reader.GetInt32(2), ""),
                            reader.GetString(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6),
                            reader.GetDateTime(7));
                    }
                    return versionControl;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Updates a VersionControl
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(VersionControl updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateVersionControl];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@ProjectInformationId", updateEntity.ProjectInformation.Id),
                    new SqlParameter("@ReleaseTypeId", updateEntity.ReleaseType.Id),
                    new SqlParameter("@CommitId", updateEntity.CommitId),
                    new SqlParameter("@Major", updateEntity.Major),
                    new SqlParameter("@Minor", updateEntity.Minor),
                    new SqlParameter("@Path", updateEntity.Patch),
                    new SqlParameter("@ReleaseDateTime", updateEntity.ReleaseDateTime)
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
