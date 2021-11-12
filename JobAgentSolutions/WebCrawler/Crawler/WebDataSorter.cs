using SkpJobCrawler.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.Crawler
{
    public class WebDataSorter
    {
        private readonly ICrawler jobCrawler;

        public WebDataSorter(ICrawler jobCrawler)
        {
            this.jobCrawler = jobCrawler;
        }

        public void SortWebDataJobPage()
        {
            foreach (var htmlData in ((JobCrawler)jobCrawler).htmlDocumentsJobPages)
            {
                
            }
        }

        public void SortWebDataVacantJobs() 
        {
            foreach (var htmlData in ((JobCrawler)jobCrawler).htmlDocumentsVacantJobs)
            {

            }    
        } 

    }
}
