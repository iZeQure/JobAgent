using Dapper;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities.EntityMaps;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs.Repositories
{
    public class VacantJobRepository : IVacantJobRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly VacantJobEntityFactory _factory;

        public VacantJobRepository(ISqlDbManager sqlDbManager, VacantJobEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;
        }

        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Create))
            {
                var proc = "[JA.spCreateVacantJob]";
                var values = new
                {
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }

        public async Task<List<IVacantJob>> GetAllAsync()
        {
            List<IVacantJob> vacantJobs = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var proc = "[JA.spGetCategories]";

                var queryResult = await conn.QueryAsync<VacantJobInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        IVacantJob vacantJob = (IVacantJob)_factory.CreateEntity(nameof(VacantJob), result.Id, result.CompanyId, result.URL);

                        vacantJobs.Add(vacantJob);
                    }
                }
            }

            return vacantJobs;
        }

        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            IVacantJob vacantJob = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Basic))
            {
                var proc = "[JA.spGetCategoryById]";
                var values = new
                {
                    @vacantJobId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<VacantJobInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    vacantJob = (IVacantJob)_factory.CreateEntity(
                                nameof(VacantJob),
                                queryResult.Id, queryResult.CompanyId, queryResult.URL);
                }
            }

            return vacantJob;
        }

        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Delete))
            {
                var proc = "[JA.spRemoveVacantJob]";
                var values = new
                {
                    @vacantJobId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbConnectionType.Update))
            {
                var proc = "[JA.spUpdateVacantJob]";
                var values = new
                {
                    @vacantJobId = entity.Id,
                    @companyId = entity.CompanyId,
                    @url = entity.URL
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0)
            {
                return await GetByIdAsync(entityId);
            }

            return null;
        }
    }
}
