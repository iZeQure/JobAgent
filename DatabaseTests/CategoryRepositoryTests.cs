using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.SqlAccess;
using DataAccess.SqlAccess.Interfaces;
using Moq;
using Pocos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace DataAccessTests
{
    public class CategoryRepositoryTests
    {
        private readonly ITestOutputHelper _output;
        private readonly IDatabaseAccess _databaseAccess;

        public CategoryRepositoryTests(ITestOutputHelper output)
        {
            _output = output;
            _databaseAccess = SqlDatabaseAccess.SqlInstance;
        }

        [Fact]
        public async Task Should_GetMenu_WithA_ListOfCategories()
        {
            // Arrange
            var repo = new CategoryRepository(_databaseAccess);

            // Actual
            var result = await repo.GetAllCategoriesWithSpecializations();

            // Assert
            try
            {
                Assert.NotNull(result);
                Assert.NotEmpty(result);

                _output.WriteLine($"No Errors found.");
            }
            catch (NotNullException ex)
            {
                _output.WriteLine($"Colletion was null => {ex.Message} : {ex.Data}");
            }
            catch (NotEmptyException ex)
            {
                _output.WriteLine($"Collection was empty => {ex.Message} : {ex.Data}");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
