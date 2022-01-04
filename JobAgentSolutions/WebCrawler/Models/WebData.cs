using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class WebData
    {
        public List<string> Data { get; set; }
        public string Link { get; set; }
        public List<string> LinksFound { get; set; }
        public List<string> JobLinks { get; set; }
        public List<string> JobListLinks { get; set; }
        public string Specialization { get; set; }
        public WebData()
        {
            Data = new List<string>();
            LinksFound = new List<string>();
            JobLinks = new List<string>();
            JobListLinks = new List<string>();
        }
    }
}
