using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Database
{
    /// <summary>
    /// Represents a helper for Sql Access.
    /// </summary>
    public static class SqlHelper
    {
        private static readonly CommandBehavior _commandBehavior = CommandBehavior.CloseConnection;

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(connectionString);

            using SqlCommand cmd = new()
            {
                CommandText = commandText,
                Connection = conn,
                CommandType = commandType
            };
            cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync(cancellation);

            return await cmd.ExecuteNonQueryAsync(cancellation);
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(connectionString);

            using SqlCommand cmd = new()
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
