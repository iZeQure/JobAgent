using HtmlAgilityPack;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobPages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Models;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private IHtmlSorter _sorter;
        private ICrawler _crawler;
        
        public CrawlerManager(ICrawler crawler, IHtmlSorter sorter)
        {
            _crawler = crawler;
            _sorter = sorter;
            ((Crawler)_crawler).SetCrawlerSettings(new CrawlerSettings());
        }
        
        /// <summary>
        /// Used to get the links from the sorter
        /// Returns empty list if HtmlArraySplitOn has not been run 
        /// </summary>
        /// <returns></returns>
        public List<string> GetLinksFound()
        {
            return _sorter.LinksFromSite;
        }

        public async Task<HtmlDocument> CrawlOnSpecifiedPage(string url, PageDefinitions pageDefinition)
        {
            try
            {
                ((Crawler)_crawler).SetCrawlerStartUrl(url, pageDefinition);
                var result = await ((Crawler)_crawler).Crawl("resultater");
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Nothing found");
            }
        }
    }
}
