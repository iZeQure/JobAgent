using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Models;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private IHtmlSorter _sorter;
        private ICrawler _crawler;
        public JobUrl _jobUrl { get; set; }

        /// <summary>
        /// Gets links to crawl from db
        /// </summary>
        public void GetStartLink()
        {
            // Get links from db
        }

        public CrawlerManager(ICrawler crawler, IHtmlSorter sorter)
        {
            _crawler = crawler;
            _sorter = sorter;
            ((Crawler)_crawler).SetCrawlerSettings(new CrawlerSettings());
            _jobUrl = new JobUrl();
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
        public void SetUrl(string url, CrawlerSettings.PageDefinitions pageDefinition)
        {
            _crawler.SetCrawlerUrl(url, pageDefinition);
            _jobUrl.StartUrl = url;
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
            _crawler.SetCrawlerUrl(_jobUrl.StartUrl, UrlCutter.GetPageDefinitionFromUrl(_jobUrl.StartUrl));

            // Starts the crawler
            // returns the html
            var hmltdocument = await ((Crawler)_crawler).Crawl();
            
            // Splits the html string for easier sort
            var htmlArray = _sorter.GetHtmlArray(hmltdocument);
            _jobUrl.LinksFoundOnPage = _sorter.GetLinksFromDocument(htmlArray);
            
            var sortedUrlList = UrlCutter.SortUrlPaths(_jobUrl.LinksFoundOnPage);
            _jobUrl.LinksFoundOnPage = sortedUrlList;
            if (sortedUrlList.Count > 0)
            {
                foreach (var url in sortedUrlList)
                {
                    var baseUrl = UrlCutter.GetBaseUrl(_jobUrl.StartUrl);
                    if(url.Split('.').Length <= 1 && !UrlCutter.CheckIfLinkLeadsToAFile(baseUrl + url) && !UrlCutter.CheckIfLinkExist(url))
                    {
                        _crawler.SetCrawlerUrl(baseUrl + url, UrlCutter.GetPageDefinitionFromUrl(baseUrl + url));

                        hmltdocument = await ((Crawler)_crawler).Crawl();
                        htmlArray = _sorter.GetHtmlArray(hmltdocument);

                        // Find some logic to go through the links and add them to url list
                        var jobUrl = _sorter.HtmlArraySplitOn('a', htmlArray);

                        _jobUrl.LinksFoundOnPage = _sorter.GetLinksFromDocument(htmlArray);
                    }
                }
            }

            return await Task.FromResult(true);
        }
    }
}
