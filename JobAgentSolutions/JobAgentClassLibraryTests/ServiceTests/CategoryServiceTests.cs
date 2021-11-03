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
            _moqRepository = new CategoryRepository(manager);
        }

        [Test]
        public async Task TestMoq()
        {
            var categories = await _moqRepository.GetAllAsync();

            Assert.IsNotNull(categories);
        }
    }
}
