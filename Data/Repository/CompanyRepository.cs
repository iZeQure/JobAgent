using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        /// <summary>
        /// Create a company.
        /// </summary>
        /// <param name="create">Used to specify the data to create.</param>
        public void Create(Company create)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("CreateCompany", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", create.CVR);
            cmd.Parameters.AddWithValue("@name", create.Name);
            cmd.Parameters.AddWithValue("@url", create.URL);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>A list of <see cref="Company"/>.</returns>
        public IEnumerable<Company> GetAll()
        {
            // Initialize a temporary list of companies.
            List<Company> tempCompanies = new List<Company>();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetAllCompanies", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check if any data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempCompanies.Add(new Company()
                    {
                        Id = reader.GetInt32("Id"),
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
        public Company GetById(int id)
        {
            // Initialize temporary obj.
            Company tempCompany = new Company();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetCompanyById", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for any data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempCompany.Id = reader.GetInt32("Id");
                    tempCompany.CVR = reader.GetInt32("CVR");
                    tempCompany.Name = reader.GetString("Name");
                    tempCompany.URL = reader.GetString("URL");
                }
            }

            // Return data.
            return tempCompany;
        }

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract()
        {
            var tempCompanies = new List<Company>();

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "SELECT * FROM [Company] WHERE NOT EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandType = CommandType.Text,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        tempCompanies.Add(
                            new Company()
                            {
                                Id = r.GetInt32(0),
                                CVR = r.GetOrdinal("CVR"),
                                Name = r.GetString("Name"),
                                URL = r.GetString("URL")
                            }
                        );
                    }
                }

                return Task.FromResult(tempCompanies as IEnumerable<Company>);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        public Task<IEnumerable<Company>> GetCompaniesWithContract()
        {
            var tempCompanies = new List<Company>();

            using SqlCommand c = new SqlCommand()
            {
                CommandText = "SELECT * FROM [Company] WHERE EXISTS (SELECT * FROM [Contract] WHERE [Contract].[CompanyId] = [Company].[Id])",
                CommandType = CommandType.Text,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        tempCompanies.Add(
                            new Company()
                            {
                                Id = r.GetInt32(0),
                                CVR = r.GetInt32(1),
                                Name = r.GetString(2),
                                URL = r.GetString(3)
                            });
                    }
                }

                return Task.FromResult(tempCompanies as IEnumerable<Company>);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }
        }

        /// <summary>
        /// Remove company data by id.
        /// </summary>
        /// <param name="id">Used to identify the data to remove.</param>
        public void Remove(int id)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveCompany", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", id);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update company information.
        /// </summary>
        /// <param name="update">Used to update the company information.</param>
        public void Update(Company update)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCompany", SqlDataAccess.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@id", update.Id);
            cmd.Parameters.AddWithValue("@cvr", update.CVR);
            cmd.Parameters.AddWithValue("@name", update.Name);
            cmd.Parameters.AddWithValue("@url", update.URL);

            // Open connection to database.
            SqlDataAccess.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }
    }
}
