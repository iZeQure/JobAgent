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
<<<<<<< HEAD
            Crawler crawler = new(_driver);
            //var data = await _crawlerManager.LoadDataToDatabase();

            var data = crawler.Crawl("https://pms.xn--lrepladsen-d6a.dk/vis-praktiksted/1019932318", "main-content");

            _driver.Dispose();
=======
            var data = await _crawlerManager.LoadDataToDatabase();

            _driver.Close();
>>>>>>> d609e52211d2e524853bc831354ec1f0add16389
            Debug.WriteLine("Done");
        }
    }
}
