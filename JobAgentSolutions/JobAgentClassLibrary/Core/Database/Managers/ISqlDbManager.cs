using JobAgentClassLibrary.Core.Entities;
using System.Data.SqlClient;

namespace JobAgentClassLibrary.Core.Database.Managers
{
    public interface ISqlDbManager
    {
        SqlConnection GetSqlConnection(DbConnectionType connectionType);
    }
}