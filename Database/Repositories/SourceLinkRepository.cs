using DataAccess.SqlAccess;
using DataAccess.Repositories.Interfaces;
using Pocos;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DataAccess.SqlAccess.Interfaces;

namespace DataAccess.Repositories
{
    public class SourceLinkRepository : ISourceLinkRepository
    {
        private readonly SqlDatabaseAccess _databaseAccess;

        public SourceLinkRepository(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = (SqlDatabaseAccess)databaseAccess;
        }

        /// <summary>
        /// Create new source link.
        /// </summary>
        /// <param name="sourceLink">Used to create new data.</param>
        /// <returns>True if data was created, otherwise false.</returns>
        public async Task Create(SourceLink sourceLink)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("CreateSourceLink", CommandType.StoredProcedure);

                // Define paramters.
                c.Parameters.AddWithValue("@companyId", sourceLink.Company.Identifier);
                c.Parameters.AddWithValue("@link", sourceLink.Link);

                c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Execute procedure.
                await _databaseAccess.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get all available source links.
        /// </summary>
        /// <returns>A collection of <see cref="SourceLink"/>.</returns>
        public async Task<IEnumerable<SourceLink>> GetAll()
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("GetAllSourceLinks", CommandType.StoredProcedure);

                c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Initialzie data reader.
                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

                List<SourceLink> sourceLinks = new List<SourceLink>();

                if (r.HasRows)
                {
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

                return sourceLinks;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Get source link by id.
        /// </summary>
        /// <param name="sourceLink">Used to get the specific object returned.</param>
        /// <returns>A source link, if the param is valid.</returns>
        public async Task<SourceLink> GetById(int id)
        {
            try
            {
                SourceLink sourceLinkObj = null;

                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("GetSourceLinkById", CommandType.StoredProcedure);

                // Parameters.
                c.Parameters.AddWithValue("@id", id);

                c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                // Initialize data reader.
                using SqlDataReader r = await _databaseAccess.GetSqlDataReader();

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

                string ouputValue = c.Parameters["@output"].Value as string;

                int.TryParse(ouputValue, out int returnValue);

                if (await CheckOutput(returnValue))
                {
                    return sourceLinkObj;
                }

                return null;
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Removes an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the data to remove.</param>
        /// <returns>True if data was removed, but false if dataset isn't found.</returns>
        public async Task Remove(int id)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("RemoveSourceLink", CommandType.StoredProcedure);

                // Parameters.
                c.Parameters.AddWithValue("@id", id);

                c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                await _databaseAccess.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
            }
        }

        /// <summary>
        /// Updates an existing dataset.
        /// </summary>
        /// <param name="sourceLink">Specifies the dataset to update.</param>
        /// <returns>True if data is updated, otherwise false.</returns>
        public async Task Update(SourceLink sourceLink)
        {
            try
            {
                // Initialize command obj.
                using SqlCommand c = _databaseAccess.GetCommand("UpdateSourceLink", CommandType.StoredProcedure);

                // Parameters.
                c.Parameters.AddWithValue("@id", sourceLink.Identifier);
                c.Parameters.AddWithValue("@companyId", sourceLink.Company.Identifier);
                c.Parameters.AddWithValue("@link", sourceLink.Link);

                c.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                await _databaseAccess.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            finally
            {
                _databaseAccess.CloseConnection();
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
