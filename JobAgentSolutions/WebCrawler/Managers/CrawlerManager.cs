using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Factory;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        public enum CompanyNames
        {
            praktikpladsen
        }

        private readonly Crawler _crawler;
        private readonly ICompanyService _companyService;
        private readonly ICategoryService _categoryService;
        private readonly IVacantJobService _vacantJobService;
        private readonly IJobAdvertService _jobAdvertService;
        private readonly ILogger<CrawlerManager> _logger;

        public List<string> KeyWords { get; set; }
        public List<string> Urls { get; set; }
        public List<string> ClassNames { get; set; }
        public CrawlerManager(Crawler crawler, ICompanyService companyService, ICategoryService categoryService, IVacantJobService vacantJobService, IJobAdvertService jobAdvertService, ILogger<CrawlerManager> logger)
        {
            _crawler = crawler;
            _companyService = companyService;
            _categoryService = categoryService;
            _vacantJobService = vacantJobService;
            _jobAdvertService = jobAdvertService;
            _logger = logger;

            Urls = new List<string>()
            {
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering",
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur",
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter",
                "https://pms.xn--lrepladsen-d6a.dk/soeg-opslag/0/Elektronik-%20og%20svagstr%C3%B8msuddannelsen/Elektronikfagtekniker?aftaleFilter=alle&medarbejdereFilter=alle",
                "https://pms.xn--lrepladsen-d6a.dk/soeg-opslag/0/Automatik-%20og%20procesuddannelsen/-1?aftaleFilter=alle&medarbejdereFilter=alle"
            };

            ClassNames = new List<string>()
            {
                "opslag-container-container"
            };
        }

        /// <summary>
        /// This method will geta all the needed data from the web
        /// And save the data to db
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunCrawlerAsync()
        {
            VacantJobEntityFactory vacantJobEntityFactory = new VacantJobEntityFactory();
            JobAdvertEntityFactory jobAdvertEntityFactory = new JobAdvertEntityFactory();

            var companies = await _companyService.GetAllAsync();
            var categories = await _categoryService.GetCategoriesAsync();
            var specialization = await _categoryService.GetSpecializationsAsync();

            var jobAdverts = await GetJobAdverts();
            var jobAdvertLinks = await GetJobAdvertLinks();

            for (int i = 0; i < jobAdverts.Count; i++)
            {
                var Company = companies.FirstOrDefault(x => x.Name.ToLower() == GetCompanyNameFromWebSite(jobAdverts[i].Url).ToString());

                //var newlyCreatedVacantJob = vacantJobEntityFactory.CreateEntity(nameof(VacantJob), 1, Company.Id, jobAdvertLinks[i].Text);
                
                // VacantJob
                var vacantId = 1;
                var vacantCompanyId = Company.Id;
                var vacantUrl = jobAdvertLinks[i].Text;

                var jobAdvertSpecialization = await GetSpecialization(jobAdvertLinks[i].Text);

                // JobAdvert
                var jobAdvertId = vacantId;
                var categoryId = categories.FirstOrDefault(x => x.Name == UrlCutter.GetCategoryFromUrl(jobAdverts[i].Url, categories)).Id;
                var specializationId = specialization.FirstOrDefault(x => x.Name == jobAdvertSpecialization.Specialization).Id;
                var title = GetTitleFromJobAdvert(jobAdvertLinks[i].Text);
                var summary = GetSummaryFromJobAdvert(jobAdverts[i].Text);
                var date = UrlCutter.GetDateFromstring(jobAdverts[i].Text);
                var newlyCreatedJobAdvert = jobAdvertEntityFactory.CreateEntity(nameof(JobAdvert), 1, categoryId, 1, title, summary, jobAdverts[i].ExpireDate);
            }

            return false;
        }

        public async Task<string> GetSummaryFromJobAdvert(string url)
        {
            _crawler.Url = url;

            var data = await _crawler.GetJobAdvert(By.TagName("span"));
            foreach (var item in data)
            {
                return item.Text.Split('.')[0];
            }

            return data[0].Text;
            
        }

        public async Task<string> GetTitleFromJobAdvert(string url)
        {
            List<string> keyWords = new() 
            {
                "lærling",
                "elev"
            };

            string result = "";
            
            _crawler.Url = url;
            var data = await _crawler.GetJobAdvert(By.TagName("h2"));

            for (int i = 0; i < keyWords.Count; i++)
            {
                Regex regex = new(@$"\b(\w*{keyWords[i]}\w*)\b");
                foreach (var item in data)
                {
                    if (regex.IsMatch(item.Text.ToLower()))
                    {
                        return item.Text;
                    }
                }
            }
            return result;
        }

        public async Task<List<WebData>> GetJobAdverts()
        {
            Task<List<WebData>> task = Task<List<WebData>>.Factory.StartNew(() =>
            {
                List<WebData> data = new();

                foreach (var url in Urls)
                {
                    _crawler.Url = url;

                    data.AddRange(_crawler.GetJobAdvert(By.ClassName(GetClassName(GetCompanyNameFromWebSite(url)))).Result);
                }

                return data;
            });

            return await Task.FromResult(await task);
        }

        /// <summary>
        /// Tasks used to get all links on sites known by the crawler
        /// </summary>
        /// <returns></returns>
        public async Task<List<WebData>> GetJobAdvertLinks()
        {
            //Starts a task that returns the links found by the crawler 
            Task<List<WebData>> task = Task<List<WebData>>.Factory.StartNew(() =>
            {
                List<WebData> links = new();
                foreach (var url in Urls)
                {
                    //sets the url for the crawler to run on
                    _crawler.Url = url;

                    // Adds the list of links found by the crawler 
                    links.AddRange(_crawler.GetJobAdvertLinks().Result);
                }

                return links;
            });

            return await Task.FromResult(await task);
        }

        public async Task<WebData> GetSpecialization(string jobAdvertLink)
        {
            List<string> specialization = new() 
            {
                "Infrastruktur",
                "Programmering",
                "It-supporter",
                "Automatikmontør",
                "Automatiktekniker",
                "Automatiseringstekniker",
                "Elektronikfagtekniker"
            };

            _crawler.Url = jobAdvertLink;
            var data = await _crawler.GetJobAdvert(By.ClassName("defs-table"));

            foreach (var item in specialization)
            {
                Regex regex = new(@$"\b(\w*{item.ToLower()}\w*)\b");
                if (regex.IsMatch(data[0].Text.ToLower()))
                {
                    data[0].Specialization = item;
                    return data[0];
                }
            }

            return data[0];
        }

        private bool CheckIfStringHasCompanyName(string companyString, CompanyNames companyName)
        {
            if (companyString == companyName.ToString())
            {
                return true;
            }

            return false;
        }

        public CompanyNames GetCompanyNameFromWebSite(string url)
        {
            var stringArray = url.Split('.');
            foreach (var item in stringArray)
            {
                foreach (var company in (CompanyNames[])Enum.GetValues(typeof(CompanyNames)))
                {
                    if (CheckIfStringHasCompanyName(item, company))
                    {
                        return company;
                    }
                }
            }

            return 0;
        }

        public string GetClassName(CompanyNames companyName)
        {
            switch (companyName)
            {
                case CompanyNames.praktikpladsen:
                    return ClassNames[0];

                default:
                    return null;
            }
        }
    }
}