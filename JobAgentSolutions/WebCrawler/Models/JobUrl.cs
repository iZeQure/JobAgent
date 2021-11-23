using System.Collections.Generic;

namespace WebCrawler.Models
{
    public class JobUrl
    {
        public string PageDefinition { get; set; }
        public string StartUrl { get; set; }
        public List<string> LinksFoundOnPage { get; set; }

        public JobUrl()
        {
            LinksFoundOnPage = new List<string>();
        }
    }
}
