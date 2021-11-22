using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private IHtmlSorter _sorter;
        private ICrawler _crawler;

        private List<List<string>> _urlsToCrawl;

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
            _urlsToCrawl = new List<List<string>>();
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
            _urlsToCrawl.Add(new List<string>() { url });
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
            foreach (var urllist in _urlsToCrawl)
            {
                foreach (var url in urllist)
                {
                    _crawler.SetCrawlerUrl(url, UrlCutter.GetPageDefinitionFromUrl(url));

                    var hmltdocument = await ((Crawler)_crawler).Crawl();
                    var htmlArray = _sorter.GetHtmlArray(hmltdocument);

                    // Find some logic to go through the links and add them to url list
                    var links = _sorter.HtmlArraySplitOn('a', htmlArray);
                }
                
            }

            return await Task.FromResult(true);
        }

    }
}
