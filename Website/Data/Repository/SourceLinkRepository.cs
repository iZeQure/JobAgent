using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.DataAccess;
using JobAgent.Data.Objects;
using JobAgent.Data.Repository.Interface;

namespace JobAgent.Data.Repository
{
    public class SourceLinkRepository : ISourceLinkRepository<SourceLink>
    {
        /// <summary>
        /// Create new source link.
        /// </summary>
        /// <param name="sourceLink">Used to create new data.</param>
        /// <returns>True if data was created, otherwise false.</returns>
        public Task<bool> Create(SourceLink sourceLink)
        {
            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Define paramters.
            c.Parameters.AddWithValue("@companyId", sourceLink.Company.Id);
            c.Parameters.AddWithValue("@link", sourceLink.Link);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            // Execute procedure.

            try
            {
                SqlDataAccess.Instance.OpenConnection();
                c.ExecuteNonQuery();

                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return CheckOutput(output);
        }

        /// <summary>
        /// Get all available source links.
        /// </summary>
        /// <returns>A collection of <see cref="SourceLink"/>.</returns>
        public Task<IEnumerable<SourceLink>> GetAll()
        {
            int output;
            List<SourceLink> sourceLinks = null;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllSourceLinks",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                // Initialzie data reader.
                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    sourceLinks = new List<SourceLink>();

                    while (r.Read())
                    {
                        sourceLinks.Add(
                            new SourceLink()
                            {
                                Id = r.GetInt32("Id"),
                                Company = new Company()
                                {
                                    Id = r.GetInt32("CompanyId"),
                                    Name = r.GetString("Name")
                                },
                                Link = r.GetString("Link")
                            });
                    }
                }
            }
            finally
            {
                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
                SqlDataAccess.Instance.CloseConnection();
            }

            if (CheckOutput(output).Result)
                return Task.FromResult((IEnumerable<SourceLink>)sourceLinks);
            else
                return Task.FromResult((IEnumerable<SourceLink>)null);
        }

        /// <summary>
        /// Get source link by id.
        /// </summary>
        /// <param name="sourceLink">Used to get the specific object returned.</param>
        /// <returns>A source link, if the param is valid.</returns>
        public Task<SourceLink> GetById(int id)
        {
            int output;
            SourceLink sourceLinkObj = null;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetSourceLinkById",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", id);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                SqlDataAccess.Instance.OpenConnection();

                // Initialize data reader.
                using SqlDataReader r = c.ExecuteReader();

                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        sourceLinkObj = new SourceLink()
                        {
                            Id = r.GetInt32("Id"),
                            Company = new Company()
                            {
                                Id = r.GetInt32("CompanyId"),
                                Name = r.GetString("Name")
                            },
                            Link = r.GetString("Link")
                        };
                    }
                }
            }
            finally
            {
                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
                SqlDataAccess.Instance.CloseConnection();
            }

            if (CheckOutput(output).Result)
                return Task.FromResult(sourceLinkObj);
            else
                return Task.FromResult((SourceLink)null);
        }

        /// <summary>
        /// Removes an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the data to remove.</param>
        /// <returns>True if data was removed, but false if dataset isn't found.</returns>
        public Task<bool> Remove(int id)
        {
            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", id);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                SqlDataAccess.Instance.OpenConnection();
                c.ExecuteNonQuery();

                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return CheckOutput(output);
        }

        /// <summary>
        /// Updates an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the dataset to update.</param>
        /// <returns>True if data is updated, otherwise false.</returns>
        public Task<bool> Update(SourceLink sourceLink)
        {
            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = SqlDataAccess.Instance.SqlConnection
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", sourceLink.Id);
            c.Parameters.AddWithValue("@companyId", sourceLink.Company.Id);
            c.Parameters.AddWithValue("@link", sourceLink.Link);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                SqlDataAccess.Instance.OpenConnection();
                c.ExecuteNonQuery();

                int.TryParse(c.Parameters["@ouput"].Value.ToString(), out output);
            }
            finally
            {
                SqlDataAccess.Instance.CloseConnection();
            }

            return CheckOutput(output);
        }

        /// <summary>
        /// Validates the output value.
        /// </summary>
        /// <param name="output">Specifies the integer to validate.</param>
        /// <returns>True if integer validates as 1, else false.</returns>
        public Task<bool> CheckOutput(int output)
        {
            if (output == 1)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
    }
}
