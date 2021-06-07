using DataAccess.SqlAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.SqlAccess
{
    /// <summary>
    /// This class handles the access to the database.
    /// </summary>
    public class SqlDatabaseAccess : IDatabaseAccess, IDisposable
    {
        private static readonly CommandBehavior _commandBehavior = CommandBehavior.Default;
        private static readonly object _connectionLock = new object();
        private static string _connectionString = string.Empty;
        private static SqlDatabaseAccess _instance = null;
        private static SqlConnection _sqlConnection = null;
        private static SqlCommand _sqlCommand = null;
        private static SqlDataReader _sqlDataReader = null;

        /// <summary>
        /// Get a new or current instance of the <see cref="SqlDatabaseAccess"/>.
        /// </summary>
        public static SqlDatabaseAccess SqlInstance
        {
            get
            {
                lock (_connectionLock)
                {
                    if (_instance == null) _instance = new SqlDatabaseAccess();

                    return _instance;
                }
            }
        }

        private SqlDatabaseAccess()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.StateChange += SqlConnection_StateChange;
            _sqlConnection.InfoMessage += SqlConnection_InfoMessage;

            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                    .Build();

                if (configuration != null)
                {
                    _connectionString = configuration.GetSection("ConnectionStrings").GetSection("Job.Agent.Database").Value;

                    if (string.IsNullOrEmpty(_connectionString) == false)
                    {
                        _sqlConnection.ConnectionString = _connectionString;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Debug.WriteLine($"SQL Info Message => {e.Message} : {e.Errors}");
        }

        private void SqlConnection_StateChange(object sender, StateChangeEventArgs e)
        {
            Debug.WriteLine($"Sql Connection State Changed => {e.OriginalState} to {e.CurrentState} on {_sqlConnection.ClientConnectionId}");
        }

        /// <summary>
        /// Get sql connection.
        /// </summary>
        /// <returns>A sql connection from the instance.</returns>
        public SqlConnection GetConnection()
        {
            return _sqlConnection;
        }

        public SqlCommand GetCommand(string commandText, CommandType commandType)
        {
            _sqlCommand = new SqlCommand { Connection = _sqlConnection };
            _sqlCommand.CommandText = commandText;
            _sqlCommand.CommandType = commandType;

            return _sqlCommand;
        }

        public async Task<SqlDataReader> GetSqlDataReader()
        {
            await OpenConnectionAsync();
            _sqlDataReader = _sqlCommand.ExecuteReader(_commandBehavior);
            return _sqlDataReader;
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (_sqlConnection != null)
                {
                    if (_sqlConnection.State.Equals(ConnectionState.Closed))
                    {
                        return;
                    }

                    _sqlConnection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Open connection
        /// </summary>
        /// <returns>Awaits to open the connection.</returns>
        public async Task OpenConnectionAsync()
        {
            try
            {
                if (_sqlConnection.State != ConnectionState.Open && _sqlConnection.State != ConnectionState.Connecting)
                {
                    await _sqlConnection.OpenAsync();
                }
                var attempts = 0;
                while (_sqlConnection.State == ConnectionState.Connecting && attempts < 10)
                {
                    attempts++;
                    Thread.Sleep(500);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
