using JobAgentClassLibrary.Core.Entities;
using System.Data.SqlClient;

namespace DataAccessLibrary.Managers
{
    public interface ISqlDbManager
    {
        SqlConnection GetSqlConnection(DbConnectionType connectionType);
    }
}