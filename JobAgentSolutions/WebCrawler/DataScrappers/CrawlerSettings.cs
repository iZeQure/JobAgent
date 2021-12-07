using System.Collections.Generic;

namespace WebCrawler.DataScrappers
{
    public class CrawlerSettings
    {
        public Dictionary<string, string> PraktikpladsenKeyWords { get; private set; } = new Dictionary<string, string>()
        {
                { "DataKey", "defs-table" },
                { "UrlKey", "resultater" }
        };

        public List<string> DrKeyWords { get; private set; } = new List<string>();
        public List<string> DiscordKeyWords { get; private set; } = new List<string>();
        public List<string> HrKeyWords { get; private set; } = new List<string>();
        public string KeyWord { get; set; }
        public string UrlToCrawl { get; set; }

        public readonly Dictionary<string, string> _baseUrls = new Dictionary<string, string>()
        {
                { "PraktikPladsen", "https://pms.praktikpladsen.dk" },
                { "Dr", "" },
                { "Discord", "" },
                { "Hr", "" }
        };


        


    }
}