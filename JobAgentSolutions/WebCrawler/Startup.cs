using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using System.Diagnostics;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly CrawlerManager _crawlerManager;
        private ChromeDriver _driver;
        public Startup(ILogger<Startup> logger, CrawlerManager manager)
        {
            _crawlerManager = manager;
        }

        public async void StartCrawler()
        {
            Crawler crawler = new Crawler(_driver);
            var data = await crawler.Crawl("https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", "SoegOpslag_searchResultEntry__1M5O9");

            foreach (var item in data.Data)
            {
                var dataArray = item.Split('/');
                foreach (var dataArr in dataArray)
                {
                    Debug.WriteLine(dataArr);
                }
                
            }


            Debug.WriteLine("Done");
        }
    }
}
