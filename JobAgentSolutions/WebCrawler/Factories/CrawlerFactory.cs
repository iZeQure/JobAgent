using Microsoft.Extensions.Configuration;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler.Factories
{
    public class CrawlerFactory
    {
        private static CrawlerFactory _factory = null;
        private static object _controle = new object();

        public static CrawlerFactory GetCrawlerFactory
        {
            get
            {
                lock (_controle)
                {
                    if (_factory is not null)
                        return _factory;
                    else
                        return new CrawlerFactory();
                }
            }
        }

        public ICrawler GetCrawler()
        {
            return new Crawler();
        }

        public IHtmlSorter GetSorter()
        {
            return new HtmlSorter();
        }

        public CrawlerManager GetCrawlerManager()
        {
            return new CrawlerManager(this);
        }

    }
}
