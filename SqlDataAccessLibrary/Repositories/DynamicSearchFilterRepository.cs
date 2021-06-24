using ObjectLibrary.Common;
using ObjectLibrary.Filter;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class DynamicSearchFilterRepository : IDynamicSearchFilterRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public DynamicSearchFilterRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a DynamicSearchFilter
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(DynamicSearchFilter createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateDynamicSearchFilter];";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@categoryId", createEntity.GetCategory.Id),
                    new SqlParameter("@specializationId", createEntity.GetSpecialization.Id),
                    new SqlParameter("@key", createEntity.GetKey)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deletes a DynamicSearchFilter
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(DynamicSearchFilter deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveDynamicSearchFilter";

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
        /// Returns a collection of all DynamicSearchFilters
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DynamicSearchFilter>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetDynamicSearchFilters]";
                List<DynamicSearchFilter> tempDynamicSearchFilterList = new();

                SqlParameter[] parameters = new SqlParameter[] { };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        DynamicSearchFilter tempDynamicSearchFilter = new(
                            id: reader.GetInt32(0),
                            category: new Category(reader.GetInt32(1), ""),
                            specialization: new Specialization(reader.GetInt32(2), new Category(0, ""), ""),
                            key: reader.GetString(3)

                            );

                        tempDynamicSearchFilterList.Add(tempDynamicSearchFilter);
                    }
                }

                return tempDynamicSearchFilterList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns specified DynamicSearchFilter by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<DynamicSearchFilter> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetStaticSearchFilterById]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    DynamicSearchFilter tempDynamicSearchFilter = null;
                    while (await reader.ReadAsync(cancellation))
                    {
                        tempDynamicSearchFilter = new(
                            id: reader.GetInt32(0),
                            category: new Category(reader.GetInt32(1), ""),
                            specialization: new Specialization(reader.GetInt32(2), new Category(0, ""), ""),
                            key: reader.GetString(3)

                            );

                    }
                    return tempDynamicSearchFilter;
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a DynamicSearchFilter
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(DynamicSearchFilter updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateDynamicSearchFilter]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@CategoryId", updateEntity.GetCategory.Id),
                    new SqlParameter("@SpecializationId", updateEntity.GetSpecialization.Id),
                    new SqlParameter("@Key", updateEntity.GetKey)
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
