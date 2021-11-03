using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Repositories;
using JobAgentClassLibraryTests.Setup;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibraryTests.RepositoryTests
{
    public class AreaRepositoryTests
    {
        IAreaRepository areaRepository;
        IArea createdArea;

        [SetUp]
        public void Setup()
        {
            var manager = SqlConfigurationSetup.SetupSqlDbManager();
            areaRepository = new AreaRepository(manager);
        }


        [Test]
        [Order(0)]
        public async Task GetAllAsync_ShouldReturnAll()
        {
            //Arrange
            bool expected = true;

            //Act
            bool actual = false;

            //Assert

            List<IArea> areaList = await areaRepository.GetAllAsync();

            if (areaList.Count() != 0 && areaList != null)
            {
                actual = true;
            }

            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Order(1)]
        public async Task CreateAsync_ShouldCreateNewArea()
        {
            //Arrange
            IArea testArea = new Area
            {
                Name = "Alabama"
            };

            //Act
            createdArea = await areaRepository.CreateAsync(testArea);

            //Assert
            Assert.IsNotNull(createdArea);
            Assert.AreEqual("Alabama", createdArea.Name);
            
        }


        [Test]
        [Order(2)]
        public async Task GetByIdAsync_ShouldReturnArea()
        {
            //Arrange
            IArea requestedArea;

            //Act
            requestedArea = await areaRepository.GetByIdAsync(createdArea.Id);

            //Assert
            Assert.IsNotNull(requestedArea);
            Assert.IsNotNull(requestedArea.Id);
            Assert.AreNotEqual(0, requestedArea.Id);
            Assert.AreEqual("Alabama", requestedArea.Name);
        }


        [Test]
        [Order(3)]
        public async Task UpdateAsync_ShouldUpdateArea()
        {
            //Arrange
            IArea updateArea;
            string newName = "Texas";

            IArea testArea = new Area
            {
                Id = createdArea.Id,
                Name = newName
            }; 
            
            updateArea = await areaRepository.UpdateAsync(testArea);

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
            bool cleanupSuccess = await areaRepository.RemoveAsync(createdArea);
            
            //Assert
            Assert.IsTrue(cleanupSuccess);
        }
    }
}
