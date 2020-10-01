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
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;

namespace JobAgent.Data.DataAccess
{
    public class SqlDataAccess : IDataAccess
    {
        #region Constructors
        public SqlDataAccess()
        {
            SqlConnection = new SqlConnection(GetConnectionString());
        }
        #endregion

        #region Attributes
        private static SqlDataAccess instance = null;
        private SqlConnection sqlConnection;
        #endregion

        #region Properites
        public SqlConnection SqlConnection { get { return sqlConnection; } private set { sqlConnection = value; } }

        public static SqlDataAccess Instance { get { if (instance == null) instance = new SqlDataAccess(); return instance; } }
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

        public void Dispose()
        {
            ((IDisposable)instance).Dispose();
        }

        public string GetConnectionString()
        {
            return DataAccessOptions.ConnectionString;
            //return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
