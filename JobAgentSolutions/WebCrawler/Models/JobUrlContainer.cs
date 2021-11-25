using JobAgentClassLibrary.Common.JobPages.Entities;
using System.Collections.Generic;
using static WebCrawler.DataScrappers.CrawlerSettings;

namespace WebCrawler.Models
{
    public class JobUrlContainer
    {
        public PageDefinitions PageDefinition { get; set; }
        public string StartUrl { get; set; }
        public List<IJobPage> LinksToCrawl { get; set; }
        public List<string> LinksFoundOnPage { get; set; }

        public JobUrlContainer()
        {
            LinksFoundOnPage = new List<string>();
            LinksToCrawl = new List<IJobPage>();
        }
    }
}
