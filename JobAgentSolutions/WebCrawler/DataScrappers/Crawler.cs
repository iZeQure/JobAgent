using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobPages.Entities;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DataSorters;
using WebCrawler.Models;

namespace WebCrawler.DataScrappers
{
    public class Crawler : ICrawler
    {
        private ChromeDriver _driver;
        public CrawlerSettings CrawlerSettings { get; private set; }
        public JobUrl JobUrl { get; private set; } = new JobUrl();
        public void SetCrawlerSettings(CrawlerSettings settings)
        {
            CrawlerSettings = settings;
        }

        public void SetCrawlerUrl(string url, CrawlerSettings.PageDefinitions pageDefinition)
        {
            JobUrl.StartUrl = url;
            CrawlerSettings.SetPageDefinitions(pageDefinition);
        }


        public async Task<HtmlDocument> Crawl(string keyWord)
        {
            Task<HtmlDocument> task = null;
            try
            {
                List<HtmlDocument> htmlDocuments = new List<HtmlDocument>();
                HtmlDocument htmlDocument = new HtmlDocument();
                task = Task.Factory.StartNew(() =>
                {
                    if (!string.IsNullOrEmpty(JobUrl.StartUrl))
                    {
                        _driver = new ChromeDriver(Environment.CurrentDirectory);
                        _driver.Navigate().GoToUrl(JobUrl.StartUrl);

                        Thread.Sleep(1000);

                        var document = _driver.FindElements(OpenQA.Selenium.By.Id(keyWord));
                        if (document.Count > 0)
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
