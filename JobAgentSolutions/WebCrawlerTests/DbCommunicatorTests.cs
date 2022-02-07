//using JobAgentClassLibrary.Common.Companies.Entities;
//using NUnit.Framework;
//using System.Linq;
//using System.Threading.Tasks;
//using WebCrawler.DataAccess;

//namespace WebCrawlerTests
//{
//    internal class DbCommunicatorTests
//    {
//        private readonly DbCommunicator DbCommunicator = new DbCommunicator();

//        [Test]
//        [Order(0)]
//        public async Task GetCompaniesAsync_HasData_IfCollectionIsNotNull()
//        {
//            // Arrange
//            ICompany firstCompany;

//            // Act
//            var companies = await DbCommunicator.GetCompaniesAsync();
//            firstCompany = companies.First();

//            // Assert
//            Assert.IsNotNull(companies);
//            Assert.IsNotEmpty(companies);
//            Assert.IsNotNull(firstCompany);
//            Assert.AreNotEqual(0, firstCompany.Id);
//            Assert.IsNotNull(firstCompany.Name);
//        }
//    }
//}
