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
                using SqlCommand cmd = _databaseAccess.GetCommand("CreateCompany", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@cvr", create.CVR);
                cmd.Parameters.AddWithValue("@name", create.Name);
                cmd.Parameters.AddWithValue("@url", create.URL);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                await cmd.ExecuteNonQueryAsync();
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
                using (SqlCommand cmd = _databaseAccess.GetCommand("GetAllCompanies", CommandType.StoredProcedure))
                {
                    // Initialize data reader.
                    using (SqlDataReader reader = await _databaseAccess.GetSqlDataReader())
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
                using SqlCommand cmd = _databaseAccess.GetCommand("GetCompanyById", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Initialize data reader.
                using SqlDataReader reader = await _databaseAccess.GetSqlDataReader();

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

            using (SqlCommand c = _databaseAccess.GetCommand("GetCompaniesWithOutContract", CommandType.StoredProcedure))
            {
                try
                {
                    SqlDataReader sqlDataReader = await _databaseAccess.GetSqlDataReader();

                    if (sqlDataReader.HasRows)
                    {
                        while (await sqlDataReader.ReadAsync())
                        {
                            tempCompanies.Add(
                                new Company()
                                {
                                    Identifier = sqlDataReader.GetInt32(0),
                                    CVR = sqlDataReader.GetOrdinal("CVR"),
                                    Name = sqlDataReader.GetString("Name"),
                                    URL = sqlDataReader.GetString("URL")
                                }
                            );
                        }
                    }

                    return tempCompanies;
                }
                finally
                {
                    _databaseAccess.CloseConnection();
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

            using SqlCommand c = _databaseAccess.GetCommand("SELECT * FROM [Company] WHERE EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])", CommandType.Text);

            try
            {
                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

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
                using SqlCommand cmd = _databaseAccess.GetCommand("RemoveCompany", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", id);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                await cmd.ExecuteNonQueryAsync();
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
                using SqlCommand cmd = _databaseAccess.GetCommand("UpdateCompany", CommandType.StoredProcedure);

                // Define input parameters.
                cmd.Parameters.AddWithValue("@id", update.Identifier);
                cmd.Parameters.AddWithValue("@cvr", update.CVR);
                cmd.Parameters.AddWithValue("@name", update.Name);
                cmd.Parameters.AddWithValue("@url", update.URL);

                // Open connection to database.
                await _databaseAccess.OpenConnectionAsync();

                // Execute command.
                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }
    }
}
