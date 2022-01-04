using HtmlAgilityPack;
using OpenQA.Selenium;
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
    public class Crawler
    {
        private IWebDriver _driver;

        public Crawler(IWebDriver webDriver)
        {
            _driver = webDriver;
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
                        _driver.Navigate().GoToUrl(url);

                        Thread.Sleep(100);

                        var document = _driver.FindElements(By.Id(keyWord));
                        webData.Link = url;

                        if (document.Count < 1)
                        {
                            document = _driver.FindElements(By.ClassName(keyWord));
                        }

                        if (document.Count > 0)
                        {
                            foreach (var item in document)
                            {
                                webData.Data.Add(new string(item.Text));
                            }
                        }

                        var links = _driver.FindElements(By.TagName("a"));

                        foreach (var link in links)
                        {
                            webData.LinksFound.Add(link.GetAttribute("href"));
                        }

                        webData.JobLinks = UrlCutter.GetJobLinks(webData.LinksFound);
                        webData.JobListLinks = UrlCutter.GetLinkLists(webData.LinksFound);
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