using HtmlAgilityPack;
using System.Threading.Tasks;

namespace WebCrawler.DataScrappers
{
    public interface ICrawler
    {
        public Task<HtmlDocument> Crawl();
        public void SetCrawlerSettings(CrawlerSettings settings);
        public void SetCrawlerUrl(string url, CrawlerSettings.PageDefinitions pageDefinition);
    }
}
