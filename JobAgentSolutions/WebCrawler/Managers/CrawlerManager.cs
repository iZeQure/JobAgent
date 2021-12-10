using HtmlAgilityPack;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Factories;

namespace WebCrawler.Managers
{
    public class CrawlerManager
    {
        private IHtmlSorter _sorter;
        private readonly DbCommunicator _dbCommunicator;
        private ICrawler _crawler;

        // Only for test 
        // Final product should get the paths from db
        private readonly List<string> mainUrlPathPraktikpladsen = new List<string>()
        {
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur"),
                new string("/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter")
        };
        
        /// <summary>
        /// This list holds the links found one the selected page
        /// </summary>
        private List<string> urlsFoundOnPraktikPladsen = new List<string>();
        public CrawlerManager(ICrawler crawler, IHtmlSorter sorter, DbCommunicator dbCommunicator)
        {
            _crawler = crawler;
            _sorter = sorter;
            _dbCommunicator = dbCommunicator;
        }

        public async Task<List<IJobPage>> GetDataFromPraktikpladsen()
        {
            var companies = await _dbCommunicator.GetCategoriesAsync();
            var categories = await _dbCommunicator.GetCategoriesAsync();
            var specializtion = await _dbCommunicator.GetSpecializationsAsync();

            foreach (var urlPath in mainUrlPathPraktikpladsen)
            {

                var links = await CrawlPraktikPladsenForLinks(urlPath);
                foreach (var item in urlsFoundOnPraktikPladsen)
                {
                    
                    VacantJob vacantJob = new VacantJob()
                    {
                        CompanyId = companies.FirstOrDefault(x => x.Name == "Praktikpladsen").Id,
                        URL = item
                    };
                    var createdVacantJob = await _dbCommunicator.CreateVacantJobAsync(vacantJob);


                    JobAdvert jobAdvert = new JobAdvert()
                    {
                        Id = createdVacantJob.Id,
                        CategoryId = categories.FirstOrDefault(c => c.Name.StartsWith("Data")).Id,

                    };

                    jobAdvert.SpecializationId = specializtion.FirstOrDefault(s => s.CategoryId == jobAdvert.CategoryId).Id;
                }

                urlsFoundOnPraktikPladsen = new List<string>();
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// This method will look for all valid links it can find on the page 
        /// Needs a start url fx https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering
        /// </summary>
        /// <param name="startUrlPath"></param>
        /// <returns></returns>
        public async Task<List<string>> CrawlPraktikPladsenForLinks(string startUrlPath)
        {
            bool linksToCrawl = true;
            int count = 0;
            List<string> linksfound = new List<string>();
            linksfound.Add(new string(startUrlPath));

            while (linksToCrawl)
            {
                if (startUrlPath != "")
                {
                    var result = await CrawlOnSpecifiedPage(((Crawler)_crawler).CrawlerSettings._baseUrls["PraktikPladsen"] + startUrlPath, "resultater");

                    // Cuts the html doc into minor pieces 
                    var htmlArray = _sorter.GetHtmlArray(result);
                    // Gets the links found in html
                    var links = _sorter.GetLinksFromDocument(htmlArray);
                    // Sorts the links so only valid link remain 
                    var sortedLinks = UrlCutter.SortUrlPaths(links);

                    count = linksfound.Count;
                    
                    linksfound.AddRange(sortedLinks);
                    linksfound = _sorter.SortPagesThatStartsWith(linksfound, "/soeg");
                    urlsFoundOnPraktikPladsen.AddRange(sortedLinks);
                    urlsFoundOnPraktikPladsen = _sorter.SortPagesThatStartsWith(urlsFoundOnPraktikPladsen, "/vis");

                    if (linksfound.Count == count)
                    {
                        linksToCrawl = false;
                    }
                }
                startUrlPath = linksfound[linksfound.Count - 1];
            }
            return linksfound;
        }

        /// <summary>
        /// Crawls the url, and looks for keyword in classes and ids
        /// </summary>
        /// <param name="url"></param>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<HtmlDocument> CrawlOnSpecifiedPage(string url, string keyWord)
        {
            try
            {
                ((Crawler)_crawler).SetCrawlerStartUrl(url);
                ((Crawler)_crawler).SetKeyWord(keyWord);

                var result = await ((Crawler)_crawler).Crawl(url, keyWord);

                if (string.IsNullOrEmpty(result.Data[0]))
                {
                    throw new Exception("Nothing Found !!");
                }
                else
                    throw new NotImplementedException();
            }
            catch (Exception)
            {
                throw new Exception("Nothing found !!");
            }
        }

        //public async Task<List<HtmlDocument>> CrawlPagesFound()
        //{
        //    DbCommunicator dbCommunicator = new DbCommunicator(_configuration);
        //    // Test to see if the call returns list of ICategory
        //    foreach (var urlPath in ((Crawler)_crawler).CrawlerSettings.JobUrl.LinksFoundOnPage)
        //    {
        //        var baseUrl = UrlCutter.GetBaseUrl(((Crawler)_crawler).CrawlerSettings.JobUrl.StartUrl);
        //        var keyWords = _crawler.CrawlerSettings.GetKeyWordsForPage();

        //        foreach (var item in keyWords)
        //        {
        //            var result = await CrawlOnSpecifiedPage(baseUrl + urlPath, UrlCutter.GetPageDefinitionFromUrl(baseUrl), item);
        //            if (!string.IsNullOrEmpty(result.Text))
        //            {
        //                _crawler.CrawlerSettings.JobUrl.HtmlDocuments.Add(result);
        //            }
        //        }
        //        // add some logic to get jobpage data from site   
        //        // only praktikpladsen for the first try 
        //    }
        //    return _crawler.CrawlerSettings.JobUrl.HtmlDocuments;
        //}
    }
}