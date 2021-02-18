using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.SqlAccess.Interfaces;
using System;

namespace DataAccess.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public CompanyRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create a company.
        /// </summary>
        /// <param name="create">Used to specify the data to create.</param>
        public async Task Create(Company create)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = new SqlCommand("CreateCompany", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@cvr", create.CVR);
                cmd.Parameters.AddWithValue("@name", create.Name);
                cmd.Parameters.AddWithValue("@url", create.URL);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>A list of <see cref="Company"/>.</returns>
        public async Task<IEnumerable<Company>> GetAll()
        {
            try
            {
                // Initialize a temporary list of companies.
                List<Company> tempCompanies = new List<Company>();

                // Initialize command obj.
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = _databaseAccess.GetConnection(),
                    CommandText = "GetAllCompanies",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    // Open connection to database.
                    await _databaseAccess.OpenConnectionAsync();

                    // Initialize data reader.
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
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
                };
            }
            catch (Exception) { throw; }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get company byd id.
        /// </summary>
        /// <param name="id">Used to indentify the dataset to return.</param>
        /// <returns>A <see cref="Company"/>.</returns>
        public async Task<Company> GetById(int id)
        {
            try
            {
                // Initialize temporary obj.
                Company tempCompany = new Company();

                // Initialize command obj.
                using SqlCommand cmd = new SqlCommand("GetCompanyById", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader reader = cmd.ExecuteReader();

                // Check for any data.
                if (reader.HasRows)
                {
                    // Read data.
                    while (reader.Read())
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
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Gets a collection of company without any associated contracts.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public async Task<IEnumerable<Company>> GetCompaniesWithOutContract()
        {
            var tempCompanies = new List<Company>();

            using (SqlCommand c = new SqlCommand()
            {
                //CommandText = "SELECT * FROM [Company] WHERE NOT EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandText = "GetCompaniesWithOutContract",
                CommandType = CommandType.StoredProcedure,
                Connection = _databaseAccess.GetConnection()
            })
            {
                try
                {
                    await _databaseAccess.OpenConnectionAsync();

                    SqlDataReader sqlDataReader = await c.ExecuteReaderAsync();

                    using SqlDataReader r = sqlDataReader;

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
                    _databaseAccess.CloseConnection();
                    await c.DisposeAsync();
                }
            };
        }

        /// <summary>
        /// Gets a collection of company where any contracts is associated.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/>.</returns>
        public async Task<IEnumerable<Company>> GetCompaniesWithContract()
        {
            var tempCompanies = new List<Company>();

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "SELECT * FROM [Company] WHERE EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandType = CommandType.Text,
                Connection = _databaseAccess.GetConnection()
            };

            try
            {
                await _databaseAccess.OpenConnectionAsync();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
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
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Remove company data by id.
        /// </summary>
        /// <param name="id">Used to identify the data to remove.</param>
        public async Task Remove(int id)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = new SqlCommand("RemoveCompany", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Update company information.
        /// </summary>
        /// <param name="update">Used to update the company information.</param>
        public async Task Update(Company update)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand cmd = new SqlCommand("UpdateCompany", _databaseAccess.GetConnection())
                {
                    CommandType = CommandType.StoredProcedure
                };

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier);
                cmd.Parameters.AddWithValue("@cvr", update.CVR);
                cmd.Parameters.AddWithValue("@name", update.Name);
                cmd.Parameters.AddWithValue("@url", update.URL);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                cmd.ExecuteNonQuery();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
