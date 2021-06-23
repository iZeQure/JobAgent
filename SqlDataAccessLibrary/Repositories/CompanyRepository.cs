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
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public CompanyRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }

        public async Task<int> CreateAsync(Company createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spCreateCompany];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@companyCVR", createEntity.CVR),
                    new SqlParameter("@companyName", createEntity.Name),
                    new SqlParameter("@contactPerson", createEntity.ContactPerson)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(Company deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spDeleteCompany];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@companyId", deleteEntity.Id)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<Company>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetCompanies];";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<Company> companies = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        companies.Add(new Company(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            reader.GetString(3)));
                    }

                    return await Task.FromResult(companies);
                }

                return await Task.FromResult(Enumerable.Empty<Company>());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Company> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spGetCompanyById];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@companyId", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Company company = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        company = new Company(
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetString(2),
                            reader.GetString(3));
                    }

                    return await Task.FromResult(company);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public Task<IEnumerable<Company>> GetCompaniesWithContract(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract(CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateAsync(Company updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "EXEC [JA.spUpdateCompany];";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@companyId", updateEntity.Id),
                    new SqlParameter("@companyCVR", updateEntity.CVR),
                    new SqlParameter("@companyName", updateEntity.Name),
                    new SqlParameter("@contactPerson", updateEntity.ContactPerson)
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
