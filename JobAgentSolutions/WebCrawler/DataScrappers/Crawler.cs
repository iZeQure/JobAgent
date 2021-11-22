using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler.DataScrappers
{
    public class Crawler : ICrawler
    {
        private ChromeDriver _driver;
        public CrawlerSettings CrawlerSettings { get; private set; }

        public void SetCrawlerSettings(CrawlerSettings settings)
        {
            CrawlerSettings = settings;
        }

        public void SetCrawlerUrl(string url, CrawlerSettings.PageDefinitions pageDefinition)
        {

            CrawlerSettings.Url = url;
            CrawlerSettings.SetPageDefinitions(pageDefinition);
        }

        public async Task<HtmlDocument> Crawl()
        {
            Task<HtmlDocument> task = null;
            try
            {
                HtmlDocument htmlDocument = new HtmlDocument();
                task = Task.Factory.StartNew(() =>
                {
                    if (!string.IsNullOrEmpty(CrawlerSettings.Url))
                    {
                        _driver = new ChromeDriver(Environment.CurrentDirectory);
                        _driver.Navigate().GoToUrl(CrawlerSettings.Url);

                        Thread.Sleep(1000);
                        var document = _driver.FindElements(OpenQA.Selenium.By.Id(CrawlerSettings.GetPageKeyWordForPage().ToString()));

                        htmlDocument.LoadHtml(document[0].GetAttribute("innerHTML"));
                        _driver.Close();
                    }

                    return htmlDocument;

                });

            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }

            return await Task.FromResult(task.Result);

        }
    }
}
