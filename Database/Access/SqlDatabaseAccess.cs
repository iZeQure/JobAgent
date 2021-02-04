using Database.Access.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Database.Access
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

        public SqlConnection GetConnection()
        {
            return _sqlConnection;
        }

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

        public async Task OpenConnectionAsync()
        {
            try
            {
                if (_sqlConnection != null)
                    await _sqlConnection.OpenAsync();
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
