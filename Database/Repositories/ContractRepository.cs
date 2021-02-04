using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ContractRepository : IContractRepository
    {
        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="create"></param>
        public async void Create(Contract create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialzie command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateContract",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Parameters.
            c.Parameters.AddWithValue("@contactPerson", create.ContactPerson);
            c.Parameters.AddWithValue("@contractName", create.ContractName);
            c.Parameters.AddWithValue("@expiryDate", create.ExpiryDate);
            c.Parameters.AddWithValue("@registeredDate", create.RegistrationDate);
            c.Parameters.AddWithValue("@signedByUserId", create.SignedByUserId.Identifier);
            c.Parameters.AddWithValue("@companyId", create.Company.Identifier);

            // Open connection to database.
            await db.OpenConnectionAsync();

            try
            {
                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                db.CloseConnection();
            }
        }

        public async Task<IEnumerable<Contract>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllContracts",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialzie data reader.
            using SqlDataReader r = await c.ExecuteReaderAsync();

            // Temporary list.
            List<Contract> tempContracts = new List<Contract>();

            // Check for any data.
            if (r.HasRows)
            {
                // Read data.
                while (await r.ReadAsync())
                {
                    tempContracts.Add(
                        new Contract()
                        {
                            Identifier = r.GetInt32("Id"),
                            ContactPerson = r.GetString("ContactPerson"),
                            ContractName = r.GetString("ContractName"),
                            ExpiryDate = r.GetDateTime("ExpiryDate"),
                            RegistrationDate = r.GetDateTime("RegisteredDate"),
                            SignedByUserId = new User()
                            {
                                Identifier = r.GetInt32("UserId"),
                                FirstName = r.GetString("FirstName"),
                                LastName = r.GetString("LastName")
                            },
                            Company = new Company()
                            {
                                Identifier = r.GetInt32("Id"),
                                CVR = r.GetInt32("CVR"),
                                Name = r.GetString("Name")
                            }
                        });
                }
            }

            // Return data list.
            return tempContracts;
        }

        public async Task<Contract> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetContractById",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@id", id);

            await db.OpenConnectionAsync();

            // Initialzie data reader.
            using SqlDataReader r = await c.ExecuteReaderAsync();

            // Temporary contract.
            Contract tempContract = new Contract();

            try
            {
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
            }
            finally
            {
                db.CloseConnection();
            }

            return tempContract;
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="id"></param>
        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveContract",
                CommandTimeout = 15,
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            c.Parameters.AddWithValue("@id", id);

            try
            {
                await db.OpenConnectionAsync();

                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Not Implemented
        /// </summary>
        /// <param name="update"></param>
        public async void Update(Contract update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateContract",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", update.Identifier);
            c.Parameters.AddWithValue("@companyId", update.Company.Identifier);
            c.Parameters.AddWithValue("@signedByUserId", update.SignedByUserId.Identifier);
            c.Parameters.AddWithValue("@contactPerson", update.ContactPerson);
            c.Parameters.AddWithValue("@regDate", update.RegistrationDate);
            c.Parameters.AddWithValue("@expiryDate", update.ExpiryDate);

            // Open connection to database.
            await db.OpenConnectionAsync();

            try
            {
                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                db.CloseConnection();
            }
        }
    }
}
