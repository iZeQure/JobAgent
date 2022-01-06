using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly CrawlerManager _crawlerManager;
        private readonly IWebDriver _driver;
        public Startup(ILogger<Startup> logger, CrawlerManager manager, IWebDriver driver)
        {
            _driver = driver;
            _crawlerManager = manager;
        }

        public async Task StartCrawlerAsync()
        {
            Crawler crawler = new(_driver);
            //var data = await _crawlerManager.LoadDataToDatabase();

            var data = crawler.Crawl("https://pms.xn--lrepladsen-d6a.dk/vis-praktiksted/1019932318", "main-content");

            _driver.Dispose();
            Debug.WriteLine("Done");
        }
    }
}
