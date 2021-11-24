using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobPages.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebCrawler.Models;

namespace WebCrawler.DataScrappers
{
    public interface ICrawler
    {
        public JobUrl JobUrl { get; }
        public Task<HtmlDocument> Crawl(string keyWord);
        public void SetCrawlerSettings(CrawlerSettings settings);
        public void SetCrawlerUrl(string url, CrawlerSettings.PageDefinitions pageDefinition);
        public void SetLinksToCrawl(List<IJobPage> jobPages);
    }
}
