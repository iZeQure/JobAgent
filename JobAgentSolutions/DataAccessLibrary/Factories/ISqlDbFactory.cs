using System.Data.SqlClient;

namespace DataAccessLibrary.Factories
{
    public interface ISqlDbFactory
    {
        SqlConnection CreateConnection(string username, string password);
    }
}