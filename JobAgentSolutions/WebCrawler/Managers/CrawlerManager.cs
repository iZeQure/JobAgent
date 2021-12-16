using JobAgentClassLibrary.Common.Categories.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Models;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private readonly DbCommunicator _dbCommunicator;
        private Crawler _crawler;

        public CrawlerManager(Crawler crawler, DbCommunicator dbCommunicator)
        {
            _crawler = crawler;
            _dbCommunicator = dbCommunicator;
        }

        // Only for test 
        // Final product should get the paths from db
        private readonly List<string> mainUrlPathPraktikpladsen = new List<string>()
        {
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter")
        };

        public async Task<List<string>> GetJobLinksFromPraktikpladsen(int pageNumber)
        {
            List<string> linkList = new();
            for (int i = pageNumber; i < 50; i++)
            {
                var data = await _crawler.Crawl($"https://pms.praktikpladsen.dk/soeg-opslag/{i}/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", "resultater");
                if (data.Data.Count != 0)
                {
                    linkList.AddRange(UrlCutter.GetLinkLists(data.LinksFound));
                }
            }

            return linkList;
        }


        public async Task<List<WebData>> GetDataFromPraktikpladsen(string startUrl)
        {
            List<WebData> webDataList = new();
            var data = await _crawler.Crawl(startUrl, "resultater");

            data.JobLinks = UrlCutter.GetJobLinks(data.LinksFound);
            data.JobListLinks = UrlCutter.GetLinkLists(data.LinksFound);

            for (int i = 0; i < data.JobListLinks.Count; i++)
            {
                WebData webData = new();
                webData = await _crawler.Crawl(data.JobListLinks[i], "resultater");
                webData.JobLinks = UrlCutter.GetJobLinks(webData.LinksFound);
                webData.JobListLinks = UrlCutter.GetLinkLists(webData.LinksFound);
                webDataList.Add(webData);
            }

            return webDataList;
        }
    }
}