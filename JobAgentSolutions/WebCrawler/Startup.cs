using OpenQA.Selenium;
using System.Diagnostics;
using System.Threading.Tasks;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Startup
    {
        private readonly CrawlerManager _crawlerManager;
        private readonly IWebDriver _driver;
        public Startup(CrawlerManager manager, IWebDriver driver)
        {
            _driver = driver;
            _crawlerManager = manager;
        }

        public async Task StartCrawlerAsync()
        {
            var data = await _crawlerManager.RunCrawlerAsync();
            _driver.Quit();
            Debug.WriteLine("Done");

        }
    }
}
