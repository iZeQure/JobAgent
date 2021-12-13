using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly UrlCutter _urlCutter;
        private readonly DbCommunicator dbCommunicator;
        private ChromeDriver _driver;
        public Startup(ILogger<Startup> logger, CrawlerManager manager, UrlCutter urlCutter, DbCommunicator dbCommunicator)
        {
            _crawlerManager = manager;
            _urlCutter = urlCutter;
            this.dbCommunicator = dbCommunicator;
        }

        public void StartCrawler()
        {
            //Crawler crawler = new Crawler(_driver);
            //var data = await crawler.Crawl("https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", "SoegOpslag_searchResultEntry__1M5O9");
            var cate = dbCommunicator.GetCategoriesAsync().Result;
           
            

            Debug.WriteLine("Done");
        }
    }
}
