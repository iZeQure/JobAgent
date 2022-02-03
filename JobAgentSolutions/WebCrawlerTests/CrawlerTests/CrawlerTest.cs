//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WebCrawler.DataScrappers;
//using WebCrawler.Factories;
//using WebCrawler.Managers;
////using WebCrawler.Models;
//using Xunit;
//using static WebCrawler.DataScrappers.CrawlerSettings;

//namespace WebCrawlerTests.CrawlerTests
//{
//    public class CrawlerTest
//    {
//        [Fact]
//        public async void Crawl_ShouldReturnSomeHtml()
//        {
//            ICrawler crawler = CrawlerFactory.GetCrawlerFactory.GetCrawler();

            
//            crawler.SetCrawlerUrl("https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", PageDefinitions.praktikpladsen);
//            var data = await ((Crawler)crawler).Crawl("resultater");

//            Assert.NotNull(data);

//        }
//    }
//}
