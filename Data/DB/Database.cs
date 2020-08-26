using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace JobAgent.Data.DB
{
    public class Database : IDatabase
    {
        #region Attributes
        private static Database instance = null;
        private SqlConnection sqlConnection;
        private string connectionString = @"";
        #endregion

        #region Properites
        public string ConnectionString { get { return connectionString; } private set { connectionString = value; } }

        public SqlConnection SqlConnection { get { return sqlConnection; } private set { sqlConnection = value; } }

        public static Database Instance { get { if (instance == null) instance = new Database(); return instance; } }
        #endregion

        #region Constructors
        public Database()
        {
            try
            {
                //ConnectionString = @"Server=10.108.48.72\SQLJOBAGENT,2009;Database=JobAgentDB; User Id=sa; Password=PaSSw0rd;";
                ConnectionString = @"Server=GFUEL\DEVSQLSERVER; Database=JobAgentDB; Integrated Security=true;";
                //ConnectionString = @"Server=VIOLURREOT\DEVELOPMENT; Database=JobAgentDB; Integrated Security=true;";

                SqlConnection = new SqlConnection(ConnectionString);
            }
            catch (Exception)
            {
                CloseConnection();
            }
        }
        #endregion

        public void OpenConnection()
        {
            try
            {
                if (SqlConnection.State != ConnectionState.Open)
                {
                    if (SqlConnection.State == ConnectionState.Connecting) return;
                    SqlConnection.Open();
                }

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (SqlConnection.State != ConnectionState.Closed) SqlConnection.Close();

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
