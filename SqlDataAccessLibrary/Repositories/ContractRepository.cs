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
    public class ContractRepository : IContractRepository
    {
        private readonly ISqlDatabase _sqlDatabase;

        public ContractRepository(ISqlDatabase sqlDatabase)
        {
            _sqlDatabase = sqlDatabase;
        }


        public async Task<int> CreateAsync(Contract createEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spCreateContract";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", createEntity.Id),
                    new SqlParameter("@userId", createEntity.user.GetUserId),
                    new SqlParameter("@name", createEntity.name),
                    new SqlParameter("@registrationDateTime", createEntity.registrationDateTime),
                    new SqlParameter("@expiryDateTime", createEntity.expiryDateTime)
                };

                return await _sqlDatabase.ExecuteNonQueryAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> DeleteAsync(Contract deleteEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spDeleteContract";
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

        public async Task<IEnumerable<Contract>> GetAllAsync(CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetContracts";

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation);

                if (reader.HasRows)
                {
                    List<Contract> contracts = new();

                    while (await reader.ReadAsync(cancellation))
                    {
                        contracts.Add(new Contract(
                            new Company(reader.GetInt32(0), 0, "", ""),
                            new User(
                                reader.GetInt32(1),
                                new Role(
                                    0, "", ""
                                    ),
                                  new Location(
                                      0, ""
                                      ), 
                                    new List<Area>(), 
                                    "", "", ""),
                            reader.GetString(2),
                            reader.GetDateTime(3),
                            reader.GetDateTime(4)
                            ));
                    }

                    return await Task.FromResult(contracts);
                }

                return await Task.FromResult(Enumerable.Empty<Contract>());
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Contract> GetByIdAsync(int id, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spGetContractById";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", id)
                };

                using var reader = await _sqlDatabase.ExecuteReaderAsync(cmdText, CommandType.StoredProcedure, cancellation, parameters);

                if (reader.HasRows)
                {
                    Contract contract = null;

                    while (await reader.ReadAsync(cancellation))
                    {
                        contract = new Contract(
                            new Company(reader.GetInt32(0), 0, "", ""),
                            new User(
                                reader.GetInt32(1),
                                new Role(
                                    0, "", ""
                                    ),
                                  new Location(
                                      0, ""
                                      ),
                                    new List<Area>(),
                                    "", "", ""),
                            reader.GetString(2),
                            reader.GetDateTime(3),
                            reader.GetDateTime(4)
                            );
                    }

                    return await Task.FromResult(contract);
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<int> UpdateAsync(Contract updateEntity, CancellationToken cancellation)
        {
            try
            {
                string cmdText = "JA.spUpdateContract";
                SqlParameter[] parameters = new[]
                {
                    new SqlParameter("@id", updateEntity.Id),
                    new SqlParameter("@userId", updateEntity.user.GetUserId),
                    new SqlParameter("@name", updateEntity.name),
                    new SqlParameter("@registrationDateTime", updateEntity.registrationDateTime),
                    new SqlParameter("@expiryDateTime", updateEntity.expiryDateTime)
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
