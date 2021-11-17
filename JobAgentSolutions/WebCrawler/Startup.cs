using Microsoft.Extensions.Logging;
using WebCrawler.DataScrappers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly ICrawler _crawler;

        public Startup(ILogger<Startup> logger, ICrawler crawler)
        {
            _logger = logger;
            _crawler = crawler;
        }

        public void StartCrawler()
        {
            _crawler.Crawl();   
        }
    }
}
