using Xunit;
using DataAccess.SqlAccess;
using System.Data;

namespace DataAccessTests
{
    public class SqlDatabaseAccessTests
    {
        [Fact]
        public async void Database_ShouldOpen_Connection()
        {
            // Arrange
            var dbContext = SqlDatabaseAccess.SqlInstance;

            await dbContext.OpenConnectionAsync();

            // Actual
            var openState = ConnectionState.Open;

            // Assertion
            Assert.True(openState == dbContext.GetConnection().State);
        }

        [Fact]
        public async void Database_ShouldClose_Connection()
        {
            // Arrange
            var dbContext = SqlDatabaseAccess.SqlInstance;

            await dbContext.OpenConnectionAsync();
            dbContext.CloseConnection();

            // Actual
            var openState = ConnectionState.Closed;

            // Assertion
            Assert.True(openState == dbContext.GetConnection().State);
        }
    }
}
