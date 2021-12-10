using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.Models;

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

        public async Task<WebData> Crawl(string url, string keyWord)
        {
            Task<WebData> task = null;
            WebData webData = new WebData();

            try
            {
                task = Task.Factory.StartNew(() =>
                {
                    if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(keyWord))
                    {
                        _driver = new ChromeDriver(Environment.CurrentDirectory);
                        _driver.Navigate().GoToUrl(url);

                        Thread.Sleep(1000);

                        var document = _driver.FindElements(By.Id(keyWord));

                        if (document.Count < 1)
                        {
                            document = _driver.FindElements(By.ClassName(keyWord));
                        }

                        if (document.Count > 0)
                        {
                            webData.Link = url;
                            foreach (var item in document)
                            {
                                webData.Data.Add(item.Text);
                            }
                        }

                        _driver.Close();
                        _driver.CloseDevToolsSession();
                        _driver.Dispose();
                    }

                    return webData;
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
