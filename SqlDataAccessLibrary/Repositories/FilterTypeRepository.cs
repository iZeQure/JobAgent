using ObjectLibrary.Filter;
using SqlDataAccessLibrary.Database;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories
{
    public class FilterTypeRepository : IFilterTypeRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public FilterTypeRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a FilterType
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(FilterType createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateFilterType];";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", createEntity.Id),
                    new SqlParameter("@Name", createEntity.Name),
                    new SqlParameter("@Description", createEntity.Description)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Deletes a FilterType
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(FilterType deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spRemoveFilterType]";

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
        /// Returns a collecton of all FilterTypes
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<FilterType>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetFilterTypes]";
                List<FilterType> tempFilterTypeList = new();

                SqlParameter[] parameters = new SqlParameter[] { };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        FilterType tempFilterType = new(
                            id: reader.GetInt32(0),
                            name: reader.GetString(1),
                            description: reader.GetString(2)

                            );

                        tempFilterTypeList.Add(tempFilterType);
                    }
                }

                return tempFilterTypeList;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a specific FilterType by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<FilterType> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetFilterTypeById]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", id),
                };

                using SqlDataReader reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    while (await reader.ReadAsync(cancellation))
                    {
                        FilterType tempFilterType = new(

                            id: reader.GetInt32(0),
                            name: reader.GetString(1),
                            description: reader.GetString(2)
                            );

                        return tempFilterType;
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
        /// Updates a FilterType
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(FilterType updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateFilterType]";

                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@Id", updateEntity.Id),
                    new SqlParameter("@Name", updateEntity.Name),
                    new SqlParameter("@Description", updateEntity.Description)
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
