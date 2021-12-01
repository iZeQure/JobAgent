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

        public async Task<HtmlDocument> CrawlOnSpecifiedPage(string url, PageDefinitions pageDefinition, string keyWord)
        {
            try
            {
                ((Crawler)_crawler).SetCrawlerStartUrl(url, pageDefinition);
                var result = await ((Crawler)_crawler).Crawl(keyWord);
                if (!string.IsNullOrEmpty(result.Text))
                {
                    var htmlArray = _sorter.GetHtmlArray(result);
                    ((Crawler)_crawler).CrawlerSettings.JobUrl.LinksFoundOnPage = UrlCutter.SortUrlPaths(_sorter.GetLinksFromDocument(htmlArray));
                }

                return result;
            }
            catch (Exception)
            {
                throw new Exception("Nothing found");
            }
        }

        public async Task<List<HtmlDocument>> CrawlPagesFound()
        {
            foreach (var urlPath in ((Crawler)_crawler).CrawlerSettings.JobUrl.LinksFoundOnPage)
            {
                var baseUrl = UrlCutter.GetBaseUrl(((Crawler)_crawler).CrawlerSettings.JobUrl.StartUrl);
                var keyWords = _crawler.CrawlerSettings.GetKeyWordsForPage();
                foreach (var item in keyWords)
                {
                    var result = await CrawlOnSpecifiedPage(baseUrl + urlPath, UrlCutter.GetPageDefinitionFromUrl(baseUrl), item);
                    if (!string.IsNullOrEmpty(result.Text))
                    {
                        _crawler.CrawlerSettings.JobUrl.HtmlDocuments.Add(result);
                    }
                }

                // add some logic to get jobpage data from site   
                // only praktikpladsen for the first try 
            }

            return _crawler.CrawlerSettings.JobUrl.HtmlDocuments;
        }
    }
}
