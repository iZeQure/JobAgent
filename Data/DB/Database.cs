using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        #region Constructors
        public Database(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public Database()
        {
            try
            {
                //ConnectionString = @"Server=10.108.48.72\SQLJOBAGENT,2009;Database=JobAgentDB; User Id=sa; Password=PaSSw0rd;";
                //ConnectionString = @"Server=GFUEL\DEVSQLSERVER; Database=JobAgentDB; Integrated Security=true;";
                //ConnectionString = @"Server=VIOLURREOT\DEVELOPMENT; Database=JobAgentDB; Integrated Security=true;";

                SqlConnection = new SqlConnection(Configuration.GetConnectionString("DevDB"));

                if (string.IsNullOrEmpty(SqlConnection.ConnectionString))
                {
                    //SqlConnection.ConnectionString = "Server=10.108.48.72\\SQLJOBAGENT,2009;Database=JobAgentDB; User Id=sa; Password=PaSSw0rd;";
                    //SqlConnection.ConnectionString = "Server=GFUEL\\DEVSQLSERVER; Database=JobAgentDB; Integrated Security=true;";
                    SqlConnection.ConnectionString = "Server=VIOLURREOT\\DEVELOPMENT; Database=JobAgentDB; Integrated Security=true;";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled Exception : {e.Message}");
            }
        }
        #endregion

        #region Attributes
        private static Database instance = null;
        private SqlConnection sqlConnection;
        #endregion

        #region Properites
        public SqlConnection SqlConnection { get { return sqlConnection; } private set { sqlConnection = value; } }

        public static Database Instance { get { if (instance == null) instance = new Database(); return instance; } }

        public IConfiguration Configuration { get; set; }
        #endregion        

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
    }
}
