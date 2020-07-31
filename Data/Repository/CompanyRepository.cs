using JobAgent.Data.DB;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("CreateCompany", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CompanyCVR", create.Id);
            cmd.Parameters.AddWithValue("CompanyName", create.Name);
            cmd.Parameters.AddWithValue("CompanyURL", create.URL);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns>A list of <see cref="Company"/>.</returns>
        public IEnumerable<Company> GetAll()
        {
            // Initialize a temporary list of companies.
            List<Company> tempCompanies = new List<Company>();

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetAllCompanies", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

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
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        URL = reader.GetString(2)
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

            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("GetCompanyById", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CompanyCVR", id);

            // Initialize data reader.
            using SqlDataReader reader = cmd.ExecuteReader();

            // Check for any data.
            if (reader.HasRows)
            {
                // Read data.
                while (reader.Read())
                {
                    tempCompany.Id = reader.GetInt32(0);
                    tempCompany.Name = reader.GetString(1);
                    tempCompany.URL = reader.GetString(2);
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
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("RemoveCompany", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CompanyCVR", id);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Update company information.
        /// </summary>
        /// <param name="update">Used to update the company information.</param>
        public void Update(Company update)
        {
            // Open connection to database.
            Database.Instance.OpenConnection();

            // Initialize command obj.
            using SqlCommand cmd = new SqlCommand("UpdateCommand", Database.Instance.SqlConnection)
            {
                CommandType = CommandType.StoredProcedure
            };

            // Define input parameters.
            cmd.Parameters.AddWithValue("CommandCVR", update.Id);
            cmd.Parameters.AddWithValue("CompanyName", update.Name);
            cmd.Parameters.AddWithValue("CompanyURL", update.URL);

            // Execute command, catch return code.
            int returnCode = cmd.ExecuteNonQuery();
        }
    }
}
