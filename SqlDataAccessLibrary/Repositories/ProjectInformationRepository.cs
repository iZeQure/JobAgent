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
    public class ProjectInformationRepository : IProjectInformationRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public ProjectInformationRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a ProjectInformation
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(ProjectInformation createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateProjectInformation];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", createEntity.Id),
                    new SqlParameter("@ProjectInformationId", createEntity.GetSystemName),
                    new SqlParameter("@ReleaseTypeId", createEntity.PublishedDateTime)

                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a ProjectInformation
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(ProjectInformation deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteProjectInformation];";
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
        /// Returns a collection of all ProjectInformations
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ProjectInformation>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetProjectInformation];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<ProjectInformation> projectInformations = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        projectInformations.Add(new ProjectInformation(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetDateTime(2)
                            ));
                    }

                    return await Task.FromResult(projectInformations);
                }

                return await Task.FromResult(Enumerable.Empty<ProjectInformation>());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a specific ProjectInformation by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<ProjectInformation> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetProjectInformationById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    ProjectInformation projectInformation = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        projectInformation = new ProjectInformation(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetDateTime(2));
                    }
                    return projectInformation;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates a ProjectInformation
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(ProjectInformation updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateProjectInformation];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@ProjectInformationId", updateEntity.GetSystemName),
                    new SqlParameter("@ReleaseTypeId", updateEntity.PublishedDateTime)
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
