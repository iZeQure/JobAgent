using ObjectLibrary.Common.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Database
{
    /// <summary>
    /// Represents a Sql Database Handler.
    /// </summary>
    public class SqlDatabase : ISqlDatabase
    {
        private readonly IConfigurationSettings _settings;

        public SqlDatabase(IConfigurationSettings settings)
        {
            _settings = settings;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            return await SqlHelper.ExecuteNonQueryAsync(_settings.ConnectionString, commandText, commandType, cancellation, parameters);
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            return await SqlHelper.ExecuteReaderAsync(_settings.ConnectionString, commandText, commandType, cancellation, parameters);
        }
    }
}
