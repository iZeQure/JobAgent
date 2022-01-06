using Dapper;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Categories.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly CategoryEntityFactory _factory;

        public CategoryRepository(ISqlDbManager sqlDbManager, CategoryEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<ICategory> CreateAsync(ICategory entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                var proc = "[JA.spCreateCategory]";
                var values = new
                {
                    @name = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<ICategory>> GetAllAsync()
        {
            List<ICategory> categories = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetCategories]";

                var queryResult = await conn.QueryAsync<CategoryInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ICategory category = (ICategory)_factory.CreateEntity(nameof(Category), result.Id, result.Name);

                        categories.Add(category);
                    }
                }
            }

            return categories;
        }

        public async Task<ICategory> GetByIdAsync(int id)
        {
            ICategory category = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetCategoryById]";
                var values = new
                {
                    @categoryId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<CategoryInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    category = (ICategory)_factory.CreateEntity(
                                nameof(Category),
                                queryResult.Id, queryResult.Name);
                }
            }

            return category;
        }

        public async Task<bool> RemoveAsync(ICategory entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveCategory]";
                var values = new
                {
                    @categoryId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<ICategory> UpdateAsync(ICategory entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateCategory]";
                var values = new
                {
                    @categoryId = entity.Id,
                    @categoryName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }
    }
}
