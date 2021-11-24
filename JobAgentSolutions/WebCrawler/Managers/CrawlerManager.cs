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
        private readonly IJobAdvertService _advertService;
        private readonly ICompanyService _companyService;
        private readonly IJobPageService _jobPageService;
        private ICrawler _crawler;
        
        public CrawlerManager(ICrawler crawler, IHtmlSorter sorter, IJobAdvertService advertService, ICompanyService companyService, IJobPageService jobPageService)
        {
            _crawler = crawler;
            _sorter = sorter;
            _advertService = advertService;
            _companyService = companyService;
            _jobPageService = jobPageService;
            ((Crawler)_crawler).SetCrawlerSettings(new CrawlerSettings());
            
        }

        public async void GetLinksFromDb()
        {
            _crawler.SetLinksToCrawl(await _jobPageService.GetJobPagesAsync());
        }

        /// <summary>
        /// Used to set the settings on the crawler
        /// </summary>
        /// <param name="settings"></param>
        public void SetCrawlerSettings(CrawlerSettings settings)
        {
            _crawler.SetCrawlerSettings(settings);
        }

        /// <summary>
        /// Sets the url to crawl on 
        /// needs page definition to narrow the search on the page
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pageDefinition"></param>
        public void SetUrl(string url, PageDefinitions pageDefinition)
        {
            _crawler.SetCrawlerUrl(url, pageDefinition);
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

        /// <summary>
        /// Starts the crawler loops through all links provided then stops
        /// </summary>
        public async Task<bool> StarCrawler()
        {
            GetLinksFromDb();

            var result = await CrawlOnSpecifiedPage(_crawler.JobUrl.StartUrl, UrlCutter.GetPageDefinitionFromUrl(_crawler.JobUrl.StartUrl));
            return true;
        }

        public async Task<HtmlDocument> CrawlOnSpecifiedPage(string url, PageDefinitions pageDefinition)
        {
            try
            {
                _crawler.SetCrawlerUrl(url, pageDefinition);
                var result = await ((Crawler)_crawler).Crawl("main-content");
                return result;
            }
            catch (Exception)
            {
                throw new Exception("Nothing found");
            }
        }
    }
}
