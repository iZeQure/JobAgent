using HtmlAgilityPack;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler.DataScrappers
{
    public interface ICrawler
    {
        public CrawlerSettings CrawlerSettings { get; }
        public void SetKeyWord(string keyWord);
        public void SetCrawlerStartUrl(string url);
        public Task<HtmlDocument> Crawl();
    }
}
