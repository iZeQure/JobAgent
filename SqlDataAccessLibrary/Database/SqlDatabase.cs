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
        private static readonly CommandBehavior _commandBehavior = CommandBehavior.CloseConnection;
        private readonly IConfigurationSettings _settings;

        public SqlDatabase(IConfigurationSettings settings)
        {
            _settings = settings;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            SqlConnection conn = new(_settings.ConnectionString);

            SqlCommand cmd = new()
            {
                CommandText = commandText,
                Connection = conn,
                CommandType = commandType
            };
            cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync(cancellation);

            return await cmd.ExecuteNonQueryAsync(cancellation);
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            SqlConnection conn = new(_settings.ConnectionString);

            SqlCommand cmd = new()
            {
                CommandText = commandText,
                Connection = conn,
                CommandType = commandType
            };

            cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync(cancellation);

            SqlDataReader reader = await cmd.ExecuteReaderAsync(_commandBehavior);
            return await Task.FromResult(reader);
        }
    }
}
