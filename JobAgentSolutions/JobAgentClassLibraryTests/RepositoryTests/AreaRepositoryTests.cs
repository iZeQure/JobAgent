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
        IArea testArea;

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

            if (areaList is not null && areaList!.Any())
            {
                IArea lol = areaList.FirstOrDefault();
                actual = true;
            }

            Assert.AreEqual(expected, actual);
        }


        [Test]
        [Order(1)]
        public async Task CreateAsync_ShouldCreateNewArea()
        {
            //Arrange
            testArea = new Area
            {
                Name = "Alabama"
            };

            //Act
            testArea = await areaRepository.CreateAsync(testArea);

            //Assert
            Assert.IsNotNull(testArea);
            Assert.AreEqual("Alabama", testArea.Name);
            
        }


        [Test]
        [Order(2)]
        public async Task GetByIdAsync_ShouldReturnArea()
        {
            //Arrange
            IArea requestedArea;

            //Act
            requestedArea = await areaRepository.GetByIdAsync(testArea.Id);

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

            IArea tempArea = new Area
            {
                Id = testArea.Id,
                Name = newName
            }; 
            
            updateArea = await areaRepository.UpdateAsync(tempArea);

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
            bool cleanupSuccess = await areaRepository.RemoveAsync(testArea);
            
            //Assert
            Assert.IsTrue(cleanupSuccess);
        }
    }
}
