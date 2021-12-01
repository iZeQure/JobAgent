using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobPages.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.DataScrappers
{
    public interface ICrawler
    {
        public CrawlerSettings CrawlerSettings { get; }
        public void SetCrawlerSettings(CrawlerSettings settings);
        public void SetLinksToCrawl(List<IJobPage> jobPages);
        public List<string> GetLinksFromSite();
        public void SetCrawlerStartUrl(string url, CrawlerSettings.PageDefinitions pageDefinition);
        public Task<HtmlDocument> Crawl(string keyWord);
    }
}
