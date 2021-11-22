using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Models
{
    public class JobUrl
    {
        public string PageDefinition { get; set; }
        public string StartUrl { get; set; }
        public List<string> LinksFoundOnPage { get; set; }
    }
}
