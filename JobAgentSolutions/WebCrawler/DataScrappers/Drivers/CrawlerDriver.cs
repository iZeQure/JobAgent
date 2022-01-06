using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.DataScrappers.Drivers
{
    public class CrawlerDriver
    {
        private readonly IWebDriver _webDriver;
        public int WorkDelay { get; set; }

        public CrawlerDriver(IWebDriver webDriver, int workDelay = 5)
        {
            WorkDelay = workDelay;
            _webDriver = webDriver;
        }

        public async Task<IReadOnlyCollection<IWebElement>> GetATags(string url, int sleepSeconds)
        {
            Task<IReadOnlyCollection<IWebElement>> task = Task<IReadOnlyCollection<IWebElement>>.Factory.StartNew(() =>
            {
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sleepSeconds);
                _webDriver.Navigate().GoToUrl(url);
                return _webDriver.FindElements(By.TagName("a"));
            });

            return await Task.FromResult(await task);
        }

        public async Task<IReadOnlyCollection<IWebElement>> GetDataFromWeb(string url, By by)
        {
            var webData = await GetJobAdverts(url, by, WorkDelay);
            return webData;
        }

        public async Task<IReadOnlyCollection<IWebElement>> GetJobAdverts(string url, By by, int sleepSeconds)
        {
            Task<IReadOnlyCollection<IWebElement>> webElements = Task<IReadOnlyCollection<IWebElement>>.Factory.StartNew(() =>
            {
                _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(sleepSeconds);
                _webDriver.Navigate().GoToUrl(url);
                var data = _webDriver.FindElements(by);
                return data;
            });

            return await Task.FromResult(await webElements);
        }
    }
}
