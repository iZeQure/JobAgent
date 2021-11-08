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
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public CategoryRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        /// <summary>
        /// Creates a Category
        /// </summary>
        /// <param name="createEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> CreateAsync(Category createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spCreateCategory]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@name", createEntity.Name)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes a category
        /// </summary>
        /// <param name="deleteEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(Category deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spDeleteCategory]";
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
        /// Returns a collection of all Categories
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetCategories]";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<Category> contracts = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        contracts.Add(new Category(
                            reader.GetInt32(0),
                            reader.GetString(1)));
                    }

                    return await Task.FromResult(contracts);
                }

                return await Task.FromResult(Enumerable.Empty<Category>());
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Returns a specific Category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<Category> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spGetCategoryById]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Category category = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        category = new Category(
                            reader.GetInt32(0),
                            reader.GetString(1));
                    }

                    return await Task.FromResult(category);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Updates a Category
        /// </summary>
        /// <param name="updateEntity"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Category updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "[JA.spUpdateCategory]";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@name", updateEntity.Name)
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
