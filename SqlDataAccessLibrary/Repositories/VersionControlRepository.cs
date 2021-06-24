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
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@projectInformationId", createEntity.ProjectInformation.Id),
                    new SqlParameter("@releaseTypeId", createEntity.ReleaseType.Id),
                    new SqlParameter("@commitId", createEntity.CommitId),
                    new SqlParameter("@major", createEntity.Major),
                    new SqlParameter("@minor", createEntity.Minor),
                    new SqlParameter("@patch", createEntity.Patch),
                    new SqlParameter("@releaseDateTime", createEntity.ReleaseDateTime)
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
                    new SqlParameter("@id", id)
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
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@projectInformationId", updateEntity.ProjectInformation.Id),
                    new SqlParameter("@releaseTypeId", updateEntity.ReleaseType.Id),
                    new SqlParameter("@commitId", updateEntity.CommitId),
                    new SqlParameter("@major", updateEntity.Major),
                    new SqlParameter("@Minor", updateEntity.Minor),
                    new SqlParameter("@patch", updateEntity.Patch),
                    new SqlParameter("@releaseDateTime", updateEntity.ReleaseDateTime)
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
