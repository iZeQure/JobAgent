﻿using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler.Factories
{
    public class CrawlerFactory
    {
        private static CrawlerFactory _factory = null;
        private static object controle = new object();
        
        public static CrawlerFactory GetCrawlerFactory
        {
            get
            {
                lock (controle)
                {
                    if (_factory == null)
                    {
                        _factory = new CrawlerFactory();
                        return _factory;
                    }
                    else
                    {
                        return _factory;
                    }
                }
            }
        }

        private ICrawler GetCrawler()
        {
            return new Crawler();
        }

        private IHtmlSorter GetSorter()
        {
            return new HtmlSorter();
        }

        public CrawlerManager GetCrawlerManager()
        {
            return new CrawlerManager(GetCrawler(), GetSorter());
        }
    }
}
