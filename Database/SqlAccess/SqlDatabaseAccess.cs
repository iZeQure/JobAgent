using DataAccess.SqlAccess.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.SqlAccess
{
    /// <summary>
    /// This class handles the access to the database.
    /// </summary>
    public class SqlDatabaseAccess : IDatabaseAccess
    {
        #region Fields
        private static readonly object _connectionLock = new object();
        private static string _connectionString = string.Empty;
        private static SqlDatabaseAccess _instance = null;
        private readonly SqlConnection _sqlConnection = null;
        #endregion

        #region Properties
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
        #endregion

        #region Constructors
        private SqlDatabaseAccess()
        {
            if (_sqlConnection == null)
            {
                try
                {
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false)
                        //.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.Development.json"), optional: false)
                        .Build();

                    if (configuration != null)
                    {
                        _connectionString = configuration.GetSection("ConnectionStrings").GetSection("Job.Agent.Database").Value;

                        if (string.IsNullOrEmpty(_connectionString) == false)
                            _sqlConnection = new SqlConnection(_connectionString);
                        else
                            throw new Exception($"Connection string couldn't be loaded.");
                    }
                    else
                        throw new Exception($"Configuration couldn't be loaded.");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion

        /// <summary>
        /// Get sql connection.
        /// </summary>
        /// <returns>A sql connection from the instance.</returns>
        public SqlConnection GetConnection()
        {
            return _sqlConnection;
        }

        /// <summary>
        /// Close connection.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (_sqlConnection != null)
                    _sqlConnection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Open connection
        /// </summary>
        /// <returns>Awaits </returns>
        public async Task OpenConnectionAsync()
        {
            try
            {
                if (_sqlConnection != null)
                {
                    if (_sqlConnection.State == System.Data.ConnectionState.Open) await Task.CompletedTask; 
                    else
                    await _sqlConnection.OpenAsync();

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
        }
    }
}
