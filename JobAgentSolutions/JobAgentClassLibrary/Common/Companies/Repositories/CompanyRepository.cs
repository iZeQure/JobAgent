using Dapper;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Entities.EntityMaps;
using JobAgentClassLibrary.Common.Companies.Factory;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Companies.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ISqlDbManager _sqlDbManager;
        private readonly CompanyEntityFactory _factory;


        public CompanyRepository(ISqlDbManager sqlDbManager, CompanyEntityFactory factory)
        {
            _sqlDbManager = sqlDbManager;
            _factory = factory;

        }


        public async Task<ICompany> CreateAsync(ICompany entity)
        {
            int entityId = 0;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.CreateUser))
            {
                string proc = "[JA.spCreateCompany]";

                var values = new
                {
                    @companyName = entity.Name,
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId != 0) return await GetByIdAsync(entityId);

            return null;
        }

        public async Task<List<ICompany>> GetAllAsync()
        {
            List<ICompany> companies = new();

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                string proc = "[JA.spGetCompanies]";

                var queryResult = await conn.QueryAsync<CompanyInformation>(proc, commandType: CommandType.StoredProcedure);

                if (queryResult is not null && queryResult.Any())
                {
                    foreach (var result in queryResult)
                    {
                        ICompany area = (ICompany)_factory.CreateEntity(
                                nameof(Company),
                                result.CompanyId, result.CompanyName);

                        companies.Add(area);
                    }
                }
            }

            return companies;
        }

        public async Task<ICompany> GetByIdAsync(int id)
        {
            ICompany company = null;
            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.BasicUser))
            {
                var proc = "[JA.spGetCompanyById]";
                var values = new
                {
                    @companyId = id
                };

                var queryResult = await conn.QuerySingleOrDefaultAsync<CompanyInformation>(proc, values, commandType: CommandType.StoredProcedure);

                if (queryResult is not null)
                {
                    company = (ICompany)_factory.CreateEntity(
                                nameof(Company),
                                queryResult.CompanyId, queryResult.CompanyName);
                }
            }

            return company;
        }

        public async Task<bool> RemoveAsync(ICompany entity)
        {
            bool isDeleted = false;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.DeleteUser))
            {
                var proc = "[JA.spRemoveCompany]";
                var values = new
                {
                    @companyId = entity.Id
                };

                isDeleted = (await conn.ExecuteAsync(proc, values, commandType: CommandType.StoredProcedure)) >= 1;
            }

            return isDeleted;
        }

        public async Task<ICompany> UpdateAsync(ICompany entity)
        {
            int entityId = 0;

            using (var conn = _sqlDbManager.GetSqlConnection(DbCredentialType.UpdateUser))
            {
                var proc = "[JA.spUpdateCompany]";
                var values = new
                {
                    @companyId = entity.Id,
                    @companyName = entity.Name
                };

                entityId = await conn.ExecuteScalarAsync<int>(proc, values, commandType: CommandType.StoredProcedure);
            }

            if (entityId >= 0) return await GetByIdAsync(entityId);

            return null;
        }

    }
}
