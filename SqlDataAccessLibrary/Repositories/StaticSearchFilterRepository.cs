using ObjectLibrary.Filter;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;


namespace SqlDataAccessLibrary
{
    public class StaticSearchFilterRepository : IStaticSearchFilterRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public StaticSearchFilterRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a StaticSearchFilter
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(StaticSearchFilter createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateStaticSearchFilter];";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", createEntity.Id),
                    new SqlParameter("@CategoryId", createEntity.FilterType.Id),
                    new SqlParameter("@Key", createEntity.GetKey)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deletes a StaticSearchFilter
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(StaticSearchFilter deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveStaticSearchFilter";

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
        /// Returns a collection of all StaticSearchFilters
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<StaticSearchFilter>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetStaticSearchFilters]";
                List<StaticSearchFilter> tempStaticSearchFilterList = new();

                SqlParameter[] parameters = new SqlParameter[] { };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        StaticSearchFilter tempStaticSearchFilter = new(
                            id: reader.GetInt32(0),
                            filterType: new(reader.GetInt32(1), "", ""),
                            key: reader.GetString(2)

                            );

                        tempStaticSearchFilterList.Add(tempStaticSearchFilter);
                    }
                }

                return tempStaticSearchFilterList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a specific StaticSearchFilter by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<StaticSearchFilter> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetStaticcSearchFilterById]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        StaticSearchFilter tempStaticSearchFilter = new(
                            id: reader.GetInt32(0),
                            filterType: new(reader.GetInt32(1), "", ""),
                            key: reader.GetString(2)

                            );

                        return tempStaticSearchFilter;
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
        /// Updates a StatickSearchFilter
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(StaticSearchFilter updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateStaticSearchFilter]";

                SqlParameter[] parameters = new[]
                {
                     new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@CategoryId", updateEntity.FilterType.Id),
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
