using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class SourceLinkRepository : ISourceLinkRepository
    {
        /// <summary>
        /// Create new source link.
        /// </summary>
        /// <param name="sourceLink">Used to create new data.</param>
        /// <returns>True if data was created, otherwise false.</returns>
        public async void Create(SourceLink sourceLink)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "CreateSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Define paramters.
            c.Parameters.AddWithValue("@companyId", sourceLink.Company.Identifier);
            c.Parameters.AddWithValue("@link", sourceLink.Link);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            // Execute procedure.

            try
            {
                await db.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();

                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Get all available source links.
        /// </summary>
        /// <returns>A collection of <see cref="SourceLink"/>.</returns>
        public async Task<IEnumerable<SourceLink>> GetAll()
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int output;
            List<SourceLink> sourceLinks = null;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetAllSourceLinks",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                await db.OpenConnectionAsync();

                // Initialzie data reader.
                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    sourceLinks = new List<SourceLink>();

                    while (await r.ReadAsync())
                    {
                        sourceLinks.Add(
                            new SourceLink()
                            {
                                Identifier = r.GetInt32("Id"),
                                Company = new Company()
                                {
                                    Identifier = r.GetInt32("CompanyId"),
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
                db.CloseConnection();
            }

            if (await CheckOutput(output))
                return sourceLinks;
            else
                return null;
        }

        /// <summary>
        /// Get source link by id.
        /// </summary>
        /// <param name="sourceLink">Used to get the specific object returned.</param>
        /// <returns>A source link, if the param is valid.</returns>
        public async Task<SourceLink> GetById(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int output;
            SourceLink sourceLinkObj = null;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "GetSourceLinkById",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", id);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                await db.OpenConnectionAsync();

                // Initialize data reader.
                using SqlDataReader r = await c.ExecuteReaderAsync();

                if (r.HasRows)
                {
                    while (await r.ReadAsync())
                    {
                        sourceLinkObj = new SourceLink()
                        {
                            Identifier = r.GetInt32("Id"),
                            Company = new Company()
                            {
                                Identifier = r.GetInt32("CompanyId"),
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
                db.CloseConnection();
            }

            if (await CheckOutput(output))
                return sourceLinkObj;
            else
                return null;
        }

        /// <summary>
        /// Removes an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the data to remove.</param>
        /// <returns>True if data was removed, but false if dataset isn't found.</returns>
        public async void Remove(int id)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "RemoveSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", id);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                await db.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();

                int.TryParse(c.Parameters["@output"].Value.ToString(), out output);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Updates an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the dataset to update.</param>
        /// <returns>True if data is updated, otherwise false.</returns>
        public async void Update(SourceLink sourceLink)
        {
            using var db = SqlDatabaseAccess.SqlInstance;

            int output;

            // Initialize command obj.
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "UpdateSourceLink",
                CommandType = CommandType.StoredProcedure,
                Connection = db.GetConnection()
            };

            // Parameters.
            c.Parameters.AddWithValue("@id", sourceLink.Identifier);
            c.Parameters.AddWithValue("@companyId", sourceLink.Company.Identifier);
            c.Parameters.AddWithValue("@link", sourceLink.Link);

            c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

            try
            {
                await db.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();

                int.TryParse(c.Parameters["@ouput"].Value.ToString(), out output);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        /// <summary>
        /// Validates the output value.
        /// </summary>
        /// <param name="output">Specifies the integer to validate.</param>
        /// <returns>True if integer validates as 1, else false.</returns>
        public Task<bool> CheckOutput(int input)
        {
            return input switch
            {
                1 => Task.FromResult(true),
                _ => Task.FromResult(false),
            };
        }
    }
}
