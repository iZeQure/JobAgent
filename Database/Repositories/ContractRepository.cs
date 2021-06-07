using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.SqlAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class ContractRepository : IContractRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public ContractRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create new contract.
        /// </summary>
        /// <param name="create"></param>
        public async Task Create(Contract create)
        {
            // Initialzie command obj.
            using SqlCommand cmd = _databaseAccess.GetCommand("CreateContract", CommandType.StoredProcedure);

            // Parameters.
            cmd.Parameters.AddWithValue("@contactPerson", create.ContactPerson);
            cmd.Parameters.AddWithValue("@contractName", create.ContractName);
            cmd.Parameters.AddWithValue("@expiryDate", create.ExpiryDate);
            cmd.Parameters.AddWithValue("@registeredDate", create.RegistrationDate);
            cmd.Parameters.AddWithValue("@signedByUserId", create.SignedByUserId.Identifier);
            cmd.Parameters.AddWithValue("@companyId", create.Company.Identifier);

            // Open connection to database.
            await _databaseAccess.OpenConnectionAsync();

            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = _databaseAccess.GetCommand("GetAllContracts", CommandType.StoredProcedure);

                // Initialzie data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

                // Temporary list.
                List<Contract> tempContracts = new List<Contract>();

                // Check for any data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (await reader.ReadAsync())
                    {
                        tempContracts.Add(
                            new Contract()
                            {
                                Identifier = reader.GetInt32("Id"),
                                ContactPerson = reader.GetString("ContactPerson"),
                                ContractName = reader.GetString("ContractName"),
                                ExpiryDate = reader.GetDateTime("ExpiryDate"),
                                RegistrationDate = reader.GetDateTime("RegisteredDate"),
                                SignedByUserId = new User()
                                {
                                    Identifier = reader.GetInt32("UserId"),
                                    FirstName = reader.GetString("FirstName"),
                                    LastName = reader.GetString("LastName")
                                },
                                Company = new Company()
                                {
                                    Identifier = reader.GetInt32("Id"),
                                    CVR = reader.GetInt32("CVR"),
                                    Name = reader.GetString("Name")
                                }
                            });
                    }
                }

                // Return data list.
                return tempContracts;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        public async Task<Contract> GetById(int id)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("GetContractById", CommandType.StoredProcedure);

                c.Parameters.AddWithValue("@id", id);

                // Initialzie data reader.
                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                // Temporary contract.
                Contract tempContract = new Contract();

                // Check for any data.
                if (r.HasRows)
                {
                    // Read data.
                    while (await r.ReadAsync())
                    {
                        tempContract.Identifier = r.GetInt32("Id");
                        tempContract.ContactPerson = r.GetString("ContactPerson");
                        tempContract.ContractName = r.GetString("ContractName");
                        tempContract.ExpiryDate = r.GetDateTime("ExpiryDate");
                        tempContract.RegistrationDate = r.GetDateTime("RegisteredDate");
                        tempContract.SignedByUserId =
                            new User()
                            {
                                Identifier = r.GetInt32("UserId"),
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName")
                            };
                        tempContract.Company =
                            new Company()
                            {
                                Identifier = r.GetInt32("CompanyId"),
                                CVR = r.GetInt32("CVR"),
                                Name = r.GetString("Name")
                            };
                    }
                }

                return tempContract;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Remove contract by id.
        /// </summary>
        /// <param name="id"></param>
        public async Task Remove(int id)
        {
            try
            {
                using SqlCommand c = _databaseAccess.GetCommand("RemoveContract", CommandType.StoredProcedure);

                c.Parameters.AddWithValue("@id", id);

                await _databaseAccess.OpenConnectionAsync();

                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Update existing contract.
        /// </summary>
        /// <param name="update"></param>
        public async Task Update(Contract update)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("UpdateContract", CommandType.StoredProcedure);

                // Parameters.
                c.Parameters.AddWithValue("@id", update.Identifier);
                c.Parameters.AddWithValue("@companyId", update.Company.Identifier);
                c.Parameters.AddWithValue("@signedByUserId", update.SignedByUserId.Identifier);
                c.Parameters.AddWithValue("@contactPerson", update.ContactPerson);
                c.Parameters.AddWithValue("@regDate", update.RegistrationDate);
                c.Parameters.AddWithValue("@expiryDate", update.ExpiryDate);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
