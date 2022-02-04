using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibraryTests.Setup;
using NUnit.Framework;
using System.Threading.Tasks;

namespace JobAgentClassLibraryTests.ServiceTests
{
    public class CategoryServiceTests
    {
        private ICategoryRepository _moqRepository;

        [SetUp]
        public void SetUp()
        {
            var manager = SqlConfigurationSetup.SetupSqlDbManager();
            _moqRepository = new CategoryRepository(manager, new JobAgentClassLibrary.Common.Categories.Factory.CategoryEntityFactory());
        }

        [Test]
        public async Task GetAllAsync_GetData_ShouldReturnPopulatedCollection()
        {
            // Arrange
            int collectionCount = 0;

            // Act
            var categories = await _moqRepository.GetAllSystemLogsAsync();
            AsyncTestDelegate getAllAction = async ()
                => await _moqRepository.GetAllSystemLogsAsync();

            // Assert
            Assert.IsNotNull(categories);
            Assert.IsNotEmpty(categories);
            Assert.Greater(categories.Count, collectionCount);
            Assert.DoesNotThrowAsync(getAllAction);
        }
    }
}
