using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        /// <summary>
        /// Create a company.
        /// </summary>
        /// <param name="create">Used to specify the data to create.</param>
        public async void Create(Company create)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("CreateCompany", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", create.CVR);
            cmd.Parameters.AddWithValue("@name", create.Name);
            cmd.Parameters.AddWithValue("@url", create.URL);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>A list of <see cref="Company"/>.</returns>
        public async Task<IEnumerable<Company>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize a temporary list of companies.
            List<Company> tempCompanies = new List<Company>();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetAllCompanies", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read data.
                while (await reader.ReadAsync())
                {
                    tempCompanies.Add(new Company()
                    {
                        Identifier = reader.GetInt32("Id"),
                        CVR = reader.GetInt32("CVR"),
                        Name = reader.GetString("Name"),
                        URL = reader.GetString("URL")
                    });
                }
            }

            // Return dataset.
            return tempCompanies;
        }

        /// <summary>
        /// Get company byd id.
        /// </summary>
        /// <param name="id">Used to indentify the dataset to return.</param>
        /// <returns>A <see cref="Company"/>.</returns>
        public async Task<Company> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize temporary obj.
            Company tempCompany = new Company();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetCompanyById", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Initialize data reader.
            using SqlDataReader reader = await cmd.ExecuteReaderAsync();

            // Check for any data.
            if (reader.HasRows)
            {
                // Read data.
                while (await reader.ReadAsync())
                {
                    tempCompany.Identifier = reader.GetInt32("Id");
                    tempCompany.CVR = reader.GetInt32("CVR");
                    tempCompany.Name = reader.GetString("Name");
                    tempCompany.URL = reader.GetString("URL");
                }
            }

            // Return data.
            return tempCompany;
        }

        /// <summary>
        /// Gets a collection of company without any associated contracts.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public async Task<IEnumerable<Company>> GetCompaniesWithOutContract()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            var tempCompanies = new List<Company>();

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "SELECT * FROM [Company] WHERE NOT EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandType = CommandType.Text,
                Connection = db.GetConnection()
            };

            try
            {
                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        tempCompanies.Add(
                            new Company()
                            {
                                Identifier = r.GetInt32(0),
                                CVR = r.GetOrdinal("CVR"),
                                Name = r.GetString("Name"),
                                URL = r.GetString("URL")
                            }
                        );
                    }
                }

                return tempCompanies;
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Gets a collection of company where any contracts is associated.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public async Task<IEnumerable<Company>> GetCompaniesWithContract()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            var tempCompanies = new List<Company>();

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "SELECT * FROM [Company] WHERE EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandType = CommandType.Text,
                Connection = db.GetConnection()
            };

            try
            {
                await db.OpenConnectionAsync();

                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        tempCompanies.Add(
                            new Company()
                            {
                                Identifier = r.GetInt32(0),
                                CVR = r.GetInt32(1),
                                Name = r.GetString(2),
                                URL = r.GetString(3)
                            });
                    }
                }

                return tempCompanies;
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Remove company data by id.
        /// </summary>
        /// <param name="id">Used to identify the data to remove.</param>
        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveCompany", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Update company information.
        /// </summary>
        /// <param name="update">Used to update the company information.</param>
        public async void Update(Company update)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCompany", db.GetConnection())
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Identifier);
            cmd.Parameters.AddWithValue("@cvr", update.CVR);
            cmd.Parameters.AddWithValue("@name", update.Name);
            cmd.Parameters.AddWithValue("@url", update.URL);

            // Open connection to database.
            await db.OpenConnectionAsync();

            // Execute command.
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
