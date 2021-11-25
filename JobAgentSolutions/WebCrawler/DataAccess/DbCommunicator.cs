using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebCrawler.DataAccess
{
    public class DbCommunicator
    {
        private readonly string _connection;

        public DbCommunicator(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("Default");
        }

        public List<ICategory> GetVacantJobs()
        {
            List<ICategory> categories = new List<ICategory>();

            SqlConnection sqlConnection = new SqlConnection(_connection);
            SqlCommand sqlCommand = new SqlCommand("[dbo].[JA.spGetCategories]", sqlConnection);
            sqlConnection.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            
            while (sqlDataReader.Read())
            {
                int id = (int)sqlDataReader["Id"];
                string name = (string)sqlDataReader["Name"];

                categories.Add(new Category()
                {
                    Id = id,
                    Name = name,
                    Specializations = new List<ISpecialization>()
                });
            }

            sqlConnection.CloseAsync();
            return categories;
        }
    }
}
