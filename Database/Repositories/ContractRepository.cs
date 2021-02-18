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
        /// Not Implemented
        /// </summary>
        /// <param name="create"></param>
        public async Task Create(Contract create)
        {
            // Initialzie command obj.
            using SqlCommand cmd = new SqlCommand()
            {
                CommandText = "CreateContract",
                CommandType = CommandType.StoredProcedure,
                Connection = _databaseAccess.GetConnection()
            };

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
                cmd.ExecuteNonQuery();
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
                using SqlCommand cmd = new SqlCommand()
                {
                    CommandText = "GetAllContracts",
                    CommandType = CommandType.StoredProcedure,
                    Connection = _databaseAccess.GetConnection()
                };

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialzie data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Temporary list.
                List<Contract> tempContracts = new List<Contract>();

                // Check for any data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (reader.Read())
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
                using SqlCommand c = new SqlCommand()
                {
                    CommandText = "GetContractById",
                    CommandType = CommandType.StoredProcedure,
                    Connection = _databaseAccess.GetConnection()
                };

                c.Parameters.AddWithValue("@id", id);

                await _databaseAccess.OpenConnectionAsync();

                // Initialzie data reader.
                using SqlDataReader r = c.ExecuteReader();

                // Temporary contract.
                Contract tempContract = new Contract();

                // Check for any data.
                if (r.HasRows)
                {
                    // Read data.
                    while (r.Read())
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
        /// Not Implemented
        /// </summary>
        /// <param name="id"></param>
        public async Task Remove(int id)
        {
            try
            {
                using SqlCommand c = new SqlCommand()
                {
                    CommandText = "RemoveContract",
                    CommandTimeout = 15,
                    CommandType = CommandType.StoredProcedure,
                    Connection = _databaseAccess.GetConnection()
                };

                c.Parameters.AddWithValue("@id", id);

                await _databaseAccess.OpenConnectionAsync();

                c.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="update"></param>
        public async Task Update(Contract update)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = new SqlCommand()
                {
                    CommandText = "UpdateContract",
                    CommandType = CommandType.StoredProcedure,
                    Connection = _databaseAccess.GetConnection()
                };

                // Parameters.
                c.Parameters.AddWithValue("@id", update.Identifier);
                c.Parameters.AddWithValue("@companyId", update.Company.Identifier);
                c.Parameters.AddWithValue("@signedByUserId", update.SignedByUserId.Identifier);
                c.Parameters.AddWithValue("@contactPerson", update.ContactPerson);
                c.Parameters.AddWithValue("@regDate", update.RegistrationDate);
                c.Parameters.AddWithValue("@expiryDate", update.ExpiryDate);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                c.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
