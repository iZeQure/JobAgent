using JobAgent.Data.DB;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

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
            using SqlCommand cmd = new SqlCommand("CreateCompany", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", create.Id);
            cmd.Parameters.AddWithValue("@name", create.Name);
            cmd.Parameters.AddWithValue("@url", create.URL);

            // Open connection to database.
            Database.Instance.OpenConnection();

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
            using SqlCommand cmd = new SqlCommand("GetAllCompanies", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Open connection to database.
            Database.Instance.OpenConnection();

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
                        Id = reader.GetInt32("CVR"),
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
            using SqlCommand cmd = new SqlCommand("GetCompanyById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", id);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for any data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempCompany.Id = reader.GetInt32("CVR");
                    tempCompany.Name = reader.GetString("Name");
                    tempCompany.URL = reader.GetString("URL");
                }
            }

            // Return data.
            return tempCompany;
        }

        /// <summary>
        /// Remove company data by id.
        /// </summary>
        /// <param name="id">Used to identify the data to remove.</param>
        public void Remove(int id)
        {
            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveCompany", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", id);

            // Open connection to database.
            Database.Instance.OpenConnection();

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
            using SqlCommand cmd = new SqlCommand("UpdateCommand", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("@cvr", update.Id);
            cmd.Parameters.AddWithValue("@name", update.Name);
            cmd.Parameters.AddWithValue("@url", update.URL);

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Execute command.
            cmd.ExecuteNonQuery();
        }
    }
}
