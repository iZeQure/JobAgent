using JobAgentClassLibrary.Common.Areas.Entities;
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
            _areaRepository = new AreaRepository(manager);
        }


        [Test]
        [Order(0)]
        public async Task GetAllAsync_HasData_IfCollectionIsNotNull()
        {
            // Arrange
            IArea firstArea;

            // Act
            var areas = await _areaRepository.GetAllAsync();
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

            var rnd = new Random(DateTime.Now.GetHashCode());
            string testAreaName = $"UnitTest{rnd.Next(3424, 458729)}";
            expected = new Area
            {
                Id = 0,
                Name = testAreaName
            };

            //Act
            actual = await _areaRepository.CreateAsync(expected);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Name);
            Assert.AreNotEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }


        [Test]
        [Order(2)]
        public async Task GetByIdAsync_ReturnsAValidObject_IfArgumentIsValid()
        {
            //Arrange
            IArea requestedArea;

            //Act
            requestedArea = await _areaRepository.GetByIdAsync(testArea.Id);

            //Assert
            Assert.IsNotNull(requestedArea);
            Assert.IsNotNull(requestedArea.Id);
            Assert.AreNotEqual(0, requestedArea.Id);
            Assert.AreEqual("Alabama", requestedArea.Name);
        }


        [Test]
        [Order(3)]
        public async Task UpdateAsync_UpdatesExistingObject_IfArgumentsIsValid()
        {
            //Arrange
            IArea updateArea;
            string newName = "Texas";

            IArea tempArea = new Area
            {
                Id = testArea.Id,
                Name = newName
            }; 
            
            updateArea = await _areaRepository.UpdateAsync(tempArea);

            //Assert
            Assert.IsNotNull(updateArea);
            Assert.AreEqual(newName, updateArea.Name);
        }


        [Test]
        [Order(4)]
        public async Task RemoveArea_ShouldRemoveArea()
        {
            //Arrange

            //Act
            bool cleanupSuccess = await _areaRepository.RemoveAsync(testArea);
            
            //Assert
            Assert.IsTrue(cleanupSuccess);
        }
    }
}
