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

        public WebData()
        {
            Data = new List<string>();
        }
    }
}
