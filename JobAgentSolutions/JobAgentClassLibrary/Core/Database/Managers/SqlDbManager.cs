using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Core.Entities;
using System;
using System.Data.SqlClient;

namespace JobAgentClassLibrary.Core.Database.Managers
{
    public class SqlDbManager : ISqlDbManager
    {
        private readonly ISqlDbFactory _factory;

        public SqlDbManager(ISqlDbFactory factory)
        {
            _factory = factory;
        }

        public SqlConnection GetSqlConnection(DbCredentialType connectionType) => connectionType switch
        {
            DbCredentialType.BasicUser => GetTestSqlConnection(),
            DbCredentialType.ComplexUser => GetTestSqlConnection(),
            DbCredentialType.CreateUser => GetTestSqlConnection(),
            DbCredentialType.UpdateUser => GetTestSqlConnection(),
            DbCredentialType.DeleteUser => GetTestSqlConnection(),
            _ => throw new ArgumentException("No Connection Type found with used type.", nameof(connectionType))
        };

        private SqlConnection GetTestSqlConnection() => _factory.CreateConnection("sa", "1234");

        private SqlConnection GetSqlConnectionBasicReader() => _factory.CreateConnection("BasicUserReader", "Pa$$w0rd");

        private SqlConnection GetSqlConnectionComplexSelect() => _factory.CreateConnection("ComplexUserReader", "Pa$$w0rd");

        private SqlConnection GetSqlConnectionDeletePermission() => _factory.CreateConnection("DeleteUserReader", "Pa$$w0rd");

        private SqlConnection GetSqlConnectionUpdatePermission() => _factory.CreateConnection("UpdateUserReader", "Pa$$w0rd");

        private SqlConnection GetSqlConnectionCreatePermission() => _factory.CreateConnection("CreateUserReader", "Pa$$w0rd");
    }
}
