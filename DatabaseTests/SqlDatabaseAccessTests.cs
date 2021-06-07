using Xunit;
using DataAccess.SqlAccess;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessTests
{
    public class SqlDatabaseAccessTests
    {
        [Fact]
        public async Task Database_Should_Return_OpenConnectionState()
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
        public async Task Database_Should_Return_ClosedConnectionState()
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
