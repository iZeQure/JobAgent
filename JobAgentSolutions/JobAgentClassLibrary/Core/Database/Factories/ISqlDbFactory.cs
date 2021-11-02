using System.Data.SqlClient;

namespace JobAgentClassLibrary.Core.Database.Factories
{
    public interface ISqlDbFactory
    {
        SqlConnection CreateConnection(string username, string password);
    }
}