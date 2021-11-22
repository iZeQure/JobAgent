using Microsoft.Extensions.Logging;
using WebCrawler.DataScrappers;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly CrawlerManager _crawlerManager;

        public Startup(ILogger<Startup> logger, CrawlerManager manager)
        {
            _crawlerManager = manager;
        }

        public async void StartCrawler()
        {
            _crawlerManager.SetUrl("https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", CrawlerSettings.PageDefinitions.praktikpladsen);
            var result = await _crawlerManager.StarCrawler();

        }
    }
}
