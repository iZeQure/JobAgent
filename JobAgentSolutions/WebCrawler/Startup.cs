using JobAgentClassLibrary.Common.Categories.Entities;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
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
        private readonly DbCommunicator dbCommunicator;
        private readonly IWebDriver _driver;
        public Startup(ILogger<Startup> logger, CrawlerManager manager,  DbCommunicator dbCommunicator, IWebDriver driver)
        {
            _driver = driver;
            _crawlerManager = manager;
            this.dbCommunicator = dbCommunicator;
        }

        public async void StartCrawler()
        {
            // var data = await _crawlerManager.GetDataFromPraktikpladsen("https://pms.praktikpladsen.dk/soeg-opslag/1/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering");
            var data = await _crawlerManager.GetJobLinksFromPraktikpladsen(0);
            data = UrlCutter.CheckListForDublicates(data);
            _driver.Close();
            Debug.WriteLine("Done");
        }
    }
}
