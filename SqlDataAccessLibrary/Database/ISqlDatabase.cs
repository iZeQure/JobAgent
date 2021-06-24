using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Database
{
    /// <summary>
    /// Gets access to the Sql Database methods.
    /// </summary>
    public interface ISqlDatabase
    {
        /// <summary>
        /// Executes a command text as a non query result.
        /// </summary>
        /// <param name="commandText">Describes the text to be executed.</param>
        /// <param name="commandType">The type of which the command text is formed as.</param>
        /// <param name="cancellation">Provides the ability to cancel the request.</param>
        /// <param name="parameters">Represents a collection of <see cref="SqlParameter"/>.</param>
        /// <returns>A task representing the asynchronous opereation of the execution result.</returns>
        Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters);

        /// <summary>
        /// Executes a result query which gives the reader.
        /// </summary>
        /// <param name="commandText">Describes the text to be executed.</param>
        /// <param name="commandType">The type of which the command text is formed as.</param>
        /// <param name="cancellation">Provides the ability to cancel the request.</param>
        /// <param name="parameters">Represents a collection of <see cref="SqlParameter"/>.</param>
        /// <returns>A task representing the asynchronous operation of the execute reader.</returns>
        Task<SqlDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, CancellationToken cancellation, params SqlParameter[] parameters);
    }
}