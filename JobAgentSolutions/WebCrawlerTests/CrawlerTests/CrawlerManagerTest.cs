using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Factories;
using WebCrawler.Managers;
using WebCrawler.Models;
using Xunit;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawlerTests.CrawlerTests
{
    public class CrawlerManagerTest
    {
        [Theory]
        [InlineData("", PageDefinitions.praktikpladsen)]
        public void SetCrawlerUrl_ShouldHaveAStartUrl(string url, PageDefinitions pageDefinition)
        {
            CrawlerManager crawlerManager = CrawlerFactory.GetCrawlerFactory.GetCrawlerManager();
            crawlerManager.SetUrl(url, pageDefinition);

            Assert.NotNull(crawlerManager.GetCrawlerJobUrl().StartUrl);
        }

        [Fact]
        public void SetJobUrl_ShouldReturnNewJobUrl()
        {
            CrawlerManager crawlerManager = CrawlerFactory.GetCrawlerFactory.GetCrawlerManager();
            JobUrl expected = new JobUrl()
            {
                PageDefinition = PageDefinitions.praktikpladsen,
                LinksFoundOnPage = new List<string>(),
                LinksToCrawl = new List<JobAgentClassLibrary.Common.JobPages.Entities.IJobPage>(),
                StartUrl = ""
            };

            crawlerManager.SetCrawlerJobUrl(expected);

            Assert.Equal(expected, crawlerManager.GetCrawlerJobUrl());
        }



    }
}
