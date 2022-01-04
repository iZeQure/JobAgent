using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
using WebCrawler.DataScrappers;
using WebCrawler.DataSorters;
using WebCrawler.Models;

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

        /// <summary>
        /// Gets data from jobpages uses the GetDataFromPraktikpladsen to get all links
        /// </summary>
        /// <returns></returns>
        public async Task<List<WebData>> GetWebData()
        {
            List<WebData> webData = new();
            var dataList = await GetDataFromPraktikpladsen();

            foreach (var webdata in dataList)
            {
                foreach (var item in webdata.JobLinks)
                {
                    var data = await _crawler.Crawl(item, "main-content");
                    data.JobListLinks.Add(webdata.Link);
                    string[] url = webdata.Link.Split("0");

                    data.Specialization = url[url.Length - 1];

                    // Make check for Jobadvert
                    webData.Add(data);
                }
            }

            return webData;
        }

        /// <summary>
        /// This method will crawl all job lists on praktikpladsen
        /// page number starts at 0 as default
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public async Task<List<string>> GetJobLinksFromPraktikpladsen(int pageNumber = 0)
        {
            List<string> linkList = new();
            for (int i = pageNumber; i < 50; i++)
            {
                var data = await _crawler.Crawl($"https://pms.praktikpladsen.dk/soeg-opslag/{i}/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering", "resultater");
                if (data.Data.Count != 0)
                {
                    linkList.AddRange(UrlCutter.GetLinkLists(data.LinksFound));
                }
            }

            return linkList;
        }

        /// <summary>
        /// This method will get all data from praktikpladsen 
        /// It uses GetJobLinksFromPraktikpladsen to get all links to crawl
        /// </summary>
        /// <returns></returns>
        public async Task<List<WebData>> GetDataFromPraktikpladsen()
        {
            List<WebData> webDataList = new();
            var jobListLinks = UrlCutter.CheckListForDublicates(await GetJobLinksFromPraktikpladsen(0));

            for (int i = 0; i < jobListLinks.Count; i++)
            {
                WebData webData = new();
                webData = await _crawler.Crawl(jobListLinks[i], "resultater");
                webDataList.Add(webData);
            }

            return webDataList;
        }

        public async Task<bool> LoadDataToDatabase()
        {
            Company company = new Company();
            VacantJob vacantJob = new VacantJob();
            JobAdvert jobAdvert = new JobAdvert();

            try
            {
                var companyList = _dbCommunicator.GetCompaniesAsync().Result;
                var categories =  _dbCommunicator.GetCategoriesAsync().Result;
                var specializations = _dbCommunicator.GetSpecializationsAsync().Result.Where(s => s.CategoryId == categories.First(c => c.Name.StartsWith("Data-")).Id).ToList();

                var data = await GetWebData();
                foreach (var item in data)
                {
                    using (StringReader sr = new(item.Data[0]))
                    {
                        vacantJob.CompanyId = companyList.FirstOrDefault(c => c.Name == "Praktikpladsen").Id;
                        vacantJob.URL = new string(item.Link);

                        var newVacantJob = _dbCommunicator.CreateVacantJobAsync(vacantJob).Result;

                        jobAdvert.Id = newVacantJob.Id;
                        jobAdvert.RegistrationDateTime = DateTime.UtcNow;
                        jobAdvert.CategoryId = categories.FirstOrDefault(c => c.Name.StartsWith("Data")).Id;
                        jobAdvert.SpecializationId = specializations.First(s => s.Name.ToLower() == item.Specialization).Id;

                        var newJobAdvert = _dbCommunicator.CreateAsync(jobAdvert).Result;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);

                return false;
            }
        }
    }
}