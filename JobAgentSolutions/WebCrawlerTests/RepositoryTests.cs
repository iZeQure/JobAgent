using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.DataAccess;

namespace WebCrawlerTests
{
    class RepositoryTests
    {
        private IJobAdvertRepository _jobAdvertRepository;

        [SetUp]
        public void Setup()
        {
            var manager = SqlConfigurationSetup.SetupSqlDbManager();
            var factory = new JobAdvertEntityFactory();
            _jobAdvertRepository = new JobAdvertRepository(manager, factory);
        }


        [Test]
        [Order(0)]
        public async Task GetJobAdvertsAsync_HasData_IfCollectionIsNotNull()
        {
            // Arrange
            IJobAdvert firstAdvert;

            // Act
            var jobAdverts = await _jobAdvertRepository.GetAllAsync();
            firstAdvert = jobAdverts.First();

            // Assert
            Assert.IsNotNull(jobAdverts);
            Assert.IsNotEmpty(jobAdverts);
            Assert.IsNotNull(firstAdvert);
            Assert.AreNotEqual(0, firstAdvert.Id);
            Assert.IsNotNull(firstAdvert.Title);
        }


        [Test]
        [Order(1)]
        public async Task CreateJobAdvertAsync_CreatesAnArea_IfArgumentsAreValid()
        {
            //Arrange
            IJobAdvert expected;
            IJobAdvert actual;

            expected = CreateTestJobAdvertObject();

            //Act
            actual = await _jobAdvertRepository.CreateAsync(expected);
            bool cleanUpAreaTest = await _jobAdvertRepository.RemoveAsync(actual);

            //Assert
            Assert.IsNotNull(actual);
            Assert.IsNotEmpty(actual.Title);
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.IsTrue(cleanUpAreaTest);
        }


        private static IJobAdvert CreateTestJobAdvertObject()
        {
            IJobAdvert expected;
            var rnd = new Random(DateTime.Now.GetHashCode());
            string testAdvertTitle = $"UnitTest{rnd.Next(3424, 458729)}";
            expected = new JobAdvert
            {
                Id = 6,
                CategoryId = 0,
                SpecializationId = 0,
                Title = testAdvertTitle,
                Summary = "blablabla",
                RegistrationDateTime = DateTime.Now
            };
            return expected;
        }


        [Test]
        [Order(2)]
        public async Task GetJobAdvertByIdAsync_ReturnsAValidObject_IfArgumentIsValid()
        {
            //Arrange
            IJobAdvert testAdvert;
            IJobAdvert requestedAdvert;

            testAdvert = await _jobAdvertRepository.CreateAsync(CreateTestJobAdvertObject());

            //Act
            requestedAdvert = await _jobAdvertRepository.GetByIdAsync(testAdvert.Id);

            //cleanup
            bool cleanUpAreaTest = await _jobAdvertRepository.RemoveAsync(requestedAdvert);

            //Assert
            Assert.IsNotNull(requestedAdvert);
            Assert.IsNotNull(requestedAdvert.Id);
            Assert.AreNotEqual(0, requestedAdvert.Id);
            Assert.AreEqual(testAdvert.Title, requestedAdvert.Title);
            Assert.IsTrue(cleanUpAreaTest);
        }


        [Test]
        [Order(3)]
        public async Task UpdateAsync_UpdatesExistingObject_IfArgumentsIsValid()
        {
            //Arrange
            IJobAdvert testAdvert;
            IJobAdvert newAdvert;
            IJobAdvert updatedAdvert;

            testAdvert = await _jobAdvertRepository.CreateAsync(CreateTestJobAdvertObject());

            //Act
            newAdvert = new JobAdvert
            {
                Id = testAdvert.Id,
                CategoryId = 0,
                SpecializationId = 0,
                Title = CreateTestJobAdvertObject().Title,
                Summary = "Opdateret!",
                RegistrationDateTime = DateTime.Now
            };

            updatedAdvert = await _jobAdvertRepository.UpdateAsync(newAdvert);

            //Cleanup
            bool cleanUpAreaTest = await _jobAdvertRepository.RemoveAsync(updatedAdvert);

            //Assert
            Assert.IsNotNull(updatedAdvert);
            Assert.IsNotNull(updatedAdvert.Title);
            Assert.AreEqual(newAdvert.Title, updatedAdvert.Title);
            Assert.IsTrue(cleanUpAreaTest);
        }


        [Test]
        [Order(4)]
        public async Task RemoveArea_ShouldRemoveArea()
        {
            //Arrange
            IJobAdvert testAdvert;
            testAdvert = await _jobAdvertRepository.CreateAsync(CreateTestJobAdvertObject());

            //Act
            bool cleanupSuccess = await _jobAdvertRepository.RemoveAsync(testAdvert);

            //Assert
            Assert.IsTrue(cleanupSuccess);
        }
    }
}
