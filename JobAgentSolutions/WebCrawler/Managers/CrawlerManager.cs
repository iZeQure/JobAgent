using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private readonly DbCommunicator _dbCommunicator;
        private Crawler _crawler;

        public CrawlerManager(Crawler crawler, DbCommunicator dbCommunicator)
        {
            _crawler = crawler;
            _dbCommunicator = dbCommunicator;
        }

        // Only for test 
        // Final product should get the paths from db
        private readonly List<string> mainUrlPathPraktikpladsen = new List<string>()
        {
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter")
        };
        
        
    }
}