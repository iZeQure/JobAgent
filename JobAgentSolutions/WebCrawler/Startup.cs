using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly CrawlerManager _crawlerManager;
        public Startup(ILogger<Startup> logger, CrawlerManager manager)
        {
            _crawlerManager = manager;
        }

        public async void StartCrawler()
        {
            var jobs = await _crawlerManager.GetDataFromPraktikpladsen();
            //var result = await _crawlerManager.CrawlPraktikpladsen();
            string wait = "";
        }
    }
}
