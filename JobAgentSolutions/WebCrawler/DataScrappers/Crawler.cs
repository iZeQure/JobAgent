using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DataScrappers.Drivers;
using WebCrawler.DataSorters;
using WebCrawler.Models;

namespace WebCrawler.DataScrappers
{
    public class Crawler
    {
        private readonly CrawlerDriver _driver;
        public string Url { get; set; }
        public IReadOnlyCollection<IWebElement> WebElements { get; set; }

        private readonly List<string> _keyWords;

        public Crawler(CrawlerDriver driver)
        {
            _driver = driver;
            _keyWords = new List<string>()
            {
                "vis-opslag",
                "main"
            };
        }

        public async Task<List<WebData>> GetJobAdvertLinks()
        {
            List<WebData> links = new();

            Task<List<WebData>> task = Task<List<WebData>>.Factory.StartNew(() =>
            {
                var webLinks = _driver.GetATags(Url, 5).Result;

                foreach (var item in webLinks)
                {
                    int keyWordCount = 0;
                    string stringArray = "";
                    while (string.IsNullOrEmpty(stringArray))
                    {
                        if (keyWordCount < (_keyWords.Count))
                        {
                            stringArray = item.GetAttribute("href")
                            .Split('/')
                            .FirstOrDefault(x => x.StartsWith(_keyWords[keyWordCount]));
                            keyWordCount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(stringArray))
                    {
                        links.Add(new WebData() { Url = Url, Text = item.GetAttribute("href") });
                    }
                }

                return links;
            
            });

            return await Task.FromResult(await task);
        }

        public async Task<List<WebData>> GetJobAdvert(By by)
        {
            Task<List<WebData>> task = Task<List<WebData>>.Factory.StartNew(() =>
            {
                WebElements = _driver.GetDataFromWeb(Url, by).Result;
                List<WebData> jobAdverts = new();

                foreach (var item in WebElements)
                {
                    if (!string.IsNullOrEmpty(item.Text))
                        jobAdverts.Add(new WebData() { Text = item.Text, Url = Url, ExpireDate = GetDateFromstring(item.Text) });
                }

                return jobAdverts;
            });

            return await Task.FromResult(await task);
        }

        public static DateTime GetDateFromstring(string dataString)
        {
            var dataArray = dataString.Split(' ');
            
            foreach (var line in dataArray)
            {
                if (DateTime.TryParse(line, out DateTime date))
                {
                    return date;
                }
            }

            return DateTime.MinValue;
        }
    }
}