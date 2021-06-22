using ObjectLibrary.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Database
{
    /// <summary>
    /// Represents a Sql Database Handler.
    /// </summary>
    public class SqlDatabase : ISqlDatabase
    {
        private readonly IDbConnection _dbConnection;

        public SqlDatabase(IConfigurationSettings settings)
        {
            _dbConnection = new SqlConnection(settings.ConnectionString);
        }

        /// <summary>
        /// Opens a new connection asynchronously.
        /// </summary>
        /// <param name="cancellation">A token representing the task to cancel if needed.</param>
        /// <returns>A task representing the asynchronous progress.</returns>
        public async Task OpenConnectionAsync(CancellationToken cancellation)
        {
            if (_dbConnection is SqlConnection connection)
            {
                using (connection)
                {
                    if (connection.State != ConnectionState.Open)
                    {
                        try
                        {
                            await connection.OpenAsync(cancellation);
                        }
                        catch (Exception ex)
                        {
                            await Task.FromException(ex);
                        }
                    }
                }

                await Task.CompletedTask;
            }

            await Task.FromException(new Exception("Error: 600. Could not initialize a valid connection."));
        }

        public void Dispose()
        {
            if (_dbConnection.State == ConnectionState.Closed)
            {
                return;
            }

            _dbConnection.Close();

            GC.SuppressFinalize(this);
        }
    }
}
