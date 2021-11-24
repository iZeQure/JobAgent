using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.DataScrappers;
using WebCrawler.Factories;
using WebCrawler.Managers;
using Xunit;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawlerTests.CrawlerTests
{
    public class CrawlerTest
    {
        private readonly CrawlerManager crawlerManager;

        public CrawlerTest(CrawlerManager crawlerManager)
        {
            this.crawlerManager = crawlerManager;
        }

        [Theory]
        [InlineData("", PageDefinitions.praktikpladsen)]
        public void SetCrawlerUrl(string url, PageDefinitions pageDefinition)
        {
            crawlerManager.SetUrl(url, pageDefinition);

            Assert.NotNull(crawlerManager..StartUrl);
        }
    }
}
