using HtmlAgilityPack;

namespace WebCrawler.DataScrappers
{
    public interface ICrawler
    {
        public HtmlDocument Crawl();
        public void SetCrawlerSettings(CrawlerSettings settings);
        public void SetCrawlerUrl(string url, CrawlerSettings.PageDefinitions pageDefinition);
    }
}
