using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using JobAgent.Data.Interfaces;

namespace JobAgent.Data.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        #region Constructors
        public SqlDataAccess()
        {
            //string connectionString = GetConnectionString("HomeDB");

            string connectionString = "Server=10.108.48.72\\SQLJOBAGENT,2009;Database=JobAgentDB; User Id=sa; Password=PaSSw0rd;";
            //string connectionString = "Server=GFUEL\\DEVSQLSERVER; Database=JobAgentDB; Integrated Security=true;";
            //string connectionString = "Server=VIOLURREOT\\DEVELOPMENT; Database=JobAgentDB; Integrated Security=true;";

            SqlConnection = new SqlConnection(connectionString);
        }

        public SqlDataAccess(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region Attributes
        private static SqlDataAccess instance = null;
        private SqlConnection sqlConnection;
        private readonly IConfiguration _config;
        #endregion

        #region Properites
        public SqlConnection SqlConnection { get { return sqlConnection; } private set { sqlConnection = value; } }

        public static SqlDataAccess Instance { get { if (instance == null) instance = new SqlDataAccess(); return instance; } }
        #endregion        

        public string GetConnectionString(string name)
        {
            return _config.GetConnectionString(name);
        }

        public void OpenConnection()
        {
            try
            {
                if (string.IsNullOrEmpty(SqlConnection.ConnectionString))
                    return;
                else
                {
                    if (SqlConnection.State != ConnectionState.Open)
                    {
                        if (SqlConnection.State != ConnectionState.Connecting)
                            SqlConnection.Open();
                        else
                        {
                            int counter = 10;

                            do
                            {
                                counter--;

                                if (counter == 0)
                                {
                                    if (SqlConnection.State == ConnectionState.Connecting)
                                        counter = 10;
                                    else
                                        SqlConnection.Open();
                                }
                            } while (SqlConnection.State == ConnectionState.Connecting);
                        }
                    }
                }
            }
            catch (SqlException)
            {
                Console.WriteLine($"Couldn't Open Connection.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled Exception : {e.Message}");
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (SqlConnection.State != ConnectionState.Closed) SqlConnection.Close();

                return;
            }
            catch (SqlException)
            {
                Console.WriteLine($"Couldn't Close Connection.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable)instance).Dispose();
        }
    }
}
