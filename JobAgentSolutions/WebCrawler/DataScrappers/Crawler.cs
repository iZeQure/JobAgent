using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace WebCrawler.DataScrappers
{
    public class Crawler : ICrawler
    {
        private ChromeDriver _driver;
        public CrawlerSettings CrawlerSettings { get; private set; } = new CrawlerSettings();

        public void SetCrawlerStartUrl(string url)
        {
            CrawlerSettings.UrlToCrawl = url;
        }

        public void SetKeyWord(string keyWord)
        {
            CrawlerSettings.KeyWord = keyWord;
        }

        public async Task<HtmlDocument> Crawl()
        {
            Task<HtmlDocument> task = null;
            try
            {
                List<HtmlDocument> htmlDocuments = new List<HtmlDocument>();
                HtmlDocument htmlDocument = new HtmlDocument();
                task = Task.Factory.StartNew(() =>
                {
                    if (!string.IsNullOrEmpty(CrawlerSettings.UrlToCrawl) && !string.IsNullOrEmpty(CrawlerSettings.KeyWord))
                    {
                        _driver = new ChromeDriver(Environment.CurrentDirectory);
                        _driver.Navigate().GoToUrl(CrawlerSettings.UrlToCrawl);

                        Thread.Sleep(1000);

                        var document = _driver.FindElements(By.Id(CrawlerSettings.KeyWord));

                        if (document.Count < 1)
                        {
                            document = _driver.FindElements(By.ClassName(CrawlerSettings.KeyWord));
                        }
                        
                        if(document.Count > 0)
                        {
                            htmlDocument.LoadHtml(document[0].GetAttribute("innerHTML"));
                            htmlDocuments.Add(htmlDocument);
                        }

                        _driver.Close();
                        _driver.CloseDevToolsSession();
                        _driver.Dispose();
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
