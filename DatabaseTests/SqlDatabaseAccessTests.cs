using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Database.Access;
using Database.Access.Interfaces;
using System.Data;

namespace DatabaseTests
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
