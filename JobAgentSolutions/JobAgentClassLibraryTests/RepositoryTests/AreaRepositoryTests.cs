using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Factory;
using JobAgentClassLibrary.Common.Areas.Repositories;
using JobAgentClassLibraryTests.Setup;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibraryTests.RepositoryTests
{
    /// <summary>
    /// Represents a test class for <see cref="IAreaRepository"/>.
    /// Handles the tests of all scenarios that could happen in the repository.
    /// </summary>
    public class AreaRepositoryTests
    {
        private IAreaRepository _areaRepository;

        [SetUp]
        public void Setup()
        {
            var manager = SqlConfigurationSetup.SetupSqlDbManager();
            var factory = new AreaEntityFactory();
            _areaRepository = new AreaRepository(manager, factory);
        }


        [Test]
        [Order(0)]
        public async Task GetAllAsync_HasData_IfCollectionIsNotNull()
        {
            // Arrange
            IArea firstArea;

            // Act
            var areas = await _areaRepository.GetAllSystemLogsAsync();
            firstArea = areas.First();

            // Assert
            Assert.IsNotNull(areas);
            Assert.IsNotEmpty(areas);
            Assert.IsNotNull(firstArea);
            Assert.AreNotEqual(0, firstArea.Id);
            Assert.IsNotNull(firstArea.Name);
            Assert.IsNotEmpty(firstArea.Name);
        }


        [Test]
        [Order(1)]
        public async Task CreateAsync_CreatesAnArea_IfArgumentsAreValid()
        {
            //Arrange
            IArea expected;
            IArea actual;

            expected = CreateTestAreaObject();

            //Act
            actual = await _areaRepository.CreateAsync(expected);
            bool cleanUpAreaTest = await _areaRepository.RemoveAsync(actual);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Name);
            Assert.AreNotEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.IsTrue(cleanUpAreaTest);
        }

        private static IArea CreateTestAreaObject()
        {
            IArea expected;
            var rnd = new Random(DateTime.Now.GetHashCode());
            string testAreaName = $"UnitTest{rnd.Next(3424, 458729)}";
            expected = new Area
            {
                Id = 0,
                Name = testAreaName
            };
            return expected;
        }

        [Test]
        [Order(2)]
        public async Task GetByIdAsync_ReturnsAValidObject_IfArgumentIsValid()
        {
            //Arrange
            IArea testArea;
            IArea requestedArea;

            testArea = await _areaRepository.CreateAsync(CreateTestAreaObject());
            
            //Act
            requestedArea = await _areaRepository.GetByIdAsync(testArea.Id);

            //cleanup
            bool cleanUpAreaTest = await _areaRepository.RemoveAsync(requestedArea);

            //Assert
            Assert.IsNotNull(requestedArea);
            Assert.IsNotNull(requestedArea.Id);
            Assert.AreNotEqual(0, requestedArea.Id);
            Assert.AreEqual(testArea.Name, requestedArea.Name);
            Assert.IsTrue(cleanUpAreaTest);
        }


        [Test]
        [Order(3)]
        public async Task UpdateAsync_UpdatesExistingObject_IfArgumentsIsValid()
        {
            //Arrange
            IArea testArea;
            IArea newArea;
            IArea updatedArea;

            testArea = await _areaRepository.CreateAsync(CreateTestAreaObject());

            //Act
            newArea = new Area
            {
                Id = testArea.Id,
                Name = CreateTestAreaObject().Name
            };
            
            updatedArea = await _areaRepository.UpdateAsync(newArea);

            //Cleanup
            bool cleanUpAreaTest = await _areaRepository.RemoveAsync(updatedArea);

            //Assert
            Assert.IsNotNull(updatedArea);
            Assert.IsNotNull(updatedArea.Name);
            Assert.AreEqual(newArea.Name, updatedArea.Name);
            Assert.IsTrue(cleanUpAreaTest);
        }


        [Test]
        [Order(4)]
        public async Task RemoveArea_ShouldRemoveArea()
        {
            //Arrange
            IArea testArea;
            testArea = await _areaRepository.CreateAsync(CreateTestAreaObject());

            //Act
            bool cleanupSuccess = await _areaRepository.RemoveAsync(testArea);
            
            //Assert
            Assert.IsTrue(cleanupSuccess);
        }
    }
}
