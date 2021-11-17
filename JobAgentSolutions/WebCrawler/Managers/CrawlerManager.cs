using System;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private IHtmlSorter _sorter;
        private ICrawler _crawler;
        
        public void GetStartLink()
        {
            // Get links from db
        }

        public CrawlerManager(ICrawler crawler, IHtmlSorter sorter)
        {
            _crawler = crawler;
            _sorter = sorter;
        }

        public void SetCrawlerSettings(CrawlerSettings settings)
        {
            _crawler.SetCrawlerSettings(settings);
        }

        public void SetUrl(string url, CrawlerSettings.PageDefinitions pageDefinition)
        {
            _crawler.SetCrawlerUrl(url, pageDefinition);
        }
         
        public void StarCrawler()
        {
            var hmltdocument = _crawler.Crawl();
            var htmlArray = _sorter.GetHtmlArray(hmltdocument);

            _sorter.HtmlArraySplitOn('a', htmlArray);

            foreach (var item in _sorter.LinksFromSite)
            {
                Console.WriteLine(item);
            }
        }

    }
}
