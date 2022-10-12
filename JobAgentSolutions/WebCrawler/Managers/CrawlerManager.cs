using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        private readonly IDynamicSearchFilterService _dynamicSearchFilterService;
        private readonly IJobPageService _jobPageService;

        private List<WebData> _jobAdverts = new();
        private List<ICompany> _companies = new();
        private List<ICategory> _categories = new();
        private List<WebData> _jobAdvertLinks = new();
        private List<ISpecialization> _specializations = new();

        public List<string> KeyWords { get; set; }
        public List<IJobPage> JobPages { get; set; }
        public List<string> HtmlClassNames { get; set; }
        public CrawlerManager(Crawler crawler, ICompanyService companyService, ICategoryService categoryService, IVacantJobService vacantJobService, IJobAdvertService jobAdvertService, ILogger<CrawlerManager> logger, IDynamicSearchFilterService dynamicSearchFilterService, IJobPageService jobPageService)
        {
            _crawler = crawler;
            _companyService = companyService;
            _categoryService = categoryService;
            _vacantJobService = vacantJobService;
            _jobAdvertService = jobAdvertService;
            _logger = logger;
            _dynamicSearchFilterService = dynamicSearchFilterService;
            _jobPageService = jobPageService;


            JobPages = new();

            //Getting changed to get links from db
            //JobPages = new List<string>()
            //{
            //    "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering",
            //    "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur",
            //    "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter",
            //    "https://pms.xn--lrepladsen-d6a.dk/soeg-opslag/0/Elektronik-%20og%20svagstr%C3%B8msuddannelsen/Elektronikfagtekniker?aftaleFilter=alle&medarbejdereFilter=alle",
            //    "https://pms.xn--lrepladsen-d6a.dk/soeg-opslag/0/Automatik-%20og%20procesuddannelsen/-1?aftaleFilter=alle&medarbejdereFilter=alle"
            //};

            HtmlClassNames = new List<string>()
            {
                "opslag-container-container"
            };
        }

        /// <summary>
        /// If there is a match it will return the object
        /// else it returns null
        /// </summary>
        /// <param name="jobAdvertLinks"></param>
        /// <returns></returns>
        public async Task<int> CheckForDuplicatesOnVacantJob(IVacantJob jobAdvertLinks)
        {
            var dbVacantJobs = await _vacantJobService.GetAllAsync();
            var checkForDuplicates = dbVacantJobs.FirstOrDefault(x => x.URL == jobAdvertLinks.URL);
            if (checkForDuplicates is not null)
            {
                return checkForDuplicates.Id;
            }

            return 0;
        }

        /// <summary>
        /// Checks for duplicates
        /// returns the id if found else it returns 0
        /// </summary>
        /// <param name="vacantId"></param>
        /// <param name="categoryId"></param>
        /// <param name="specializationId"></param>
        /// <param name="title"></param>
        /// <param name="summary"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public IJobAdvert CreateJobAdvert(int vacantId, int categoryId, int specializationId, string title, string summary, DateTime date)
        {
            JobAdvertEntityFactory jobAdvertEntityFactory = new JobAdvertEntityFactory();
            var newlyCreatedJobAdvert = jobAdvertEntityFactory.CreateEntity(
                   nameof(JobAdvert),
                   vacantId,
                   categoryId,
                   specializationId,
                   title,
                   summary,
                   date);

            return (JobAdvert)newlyCreatedJobAdvert;
        }

        /// <summary>
        /// Checks for duplicates
        /// returns the id if found else it returns 0
        /// </summary>
        /// <param name="jobAdvert"></param>
        /// <returns></returns>
        public async Task<int> CheckForDuplicatesOnJobAdvert(IJobAdvert jobAdvert)
        {
            List<IJobAdvert> jobAdverts = await _jobAdvertService.GetJobAdvertsAsync();
            var checkForDuplicates = jobAdverts.FirstOrDefault(x => x.Title == jobAdvert.Title && x.Summary == jobAdvert.Summary);
            if (checkForDuplicates is not null)
            {
                return checkForDuplicates.Id;
            }

            return 0;
        }

        /// <summary>
        /// This method will geta all the needed data from the web
        /// And save the data to db
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RunCrawlerAsync()
        {
            VacantJobEntityFactory vacantJobEntityFactory = new VacantJobEntityFactory();

            await LoadDbData();


            var jobAdverts = await GetRawJobAdvertData();
            var jobAdvertLinks = await GetJobAdvertLinks();

            for (int i = 0; i < jobAdverts.Count; i++)
            {
                var Company = _companies.FirstOrDefault(x => x.Name.ToLower() == GetCompanyNameFromWebSite(_jobAdverts[i].Url).ToString());

                var newlyCreatedVacantJob = vacantJobEntityFactory.CreateEntity(nameof(VacantJob), 1, Company.Id, _jobAdvertLinks[i].Text);
                int vacantId = await CheckForDuplicatesOnVacantJob((IVacantJob)newlyCreatedVacantJob);

                if (vacantId is 0)
                {
                    vacantId = (await _vacantJobService.CreateAsync((IVacantJob)newlyCreatedVacantJob)).Id;
                }

                // JobAdvert
                var jobAdvertSpecialization = await GetSpecialization(_jobAdvertLinks[i].Text);

                var newlyCreatedJobAdvert = CreateJobAdvert(
                   vacantId,
                   _categories.FirstOrDefault(x => x.Name == UrlCutter.GetCategoryFromUrl(_jobAdverts[i].Url, _categories)).Id,
                   _specializations.FirstOrDefault(x => x.Name == jobAdvertSpecialization.Specialization).Id,
                   await GetTitleFromJobAdvert(_jobAdvertLinks[i].Text),
                   await GetSummaryFromJobAdvert(_jobAdvertLinks[i].Text),
                   _jobAdverts[i].ExpireDate
                   );
                //----------------------------------------------------------
                int jobAdvertId = await CheckForDuplicatesOnJobAdvert(newlyCreatedJobAdvert);
                if (jobAdvertId == 0)
                {
                    await _jobAdvertService.CreateAsync(newlyCreatedJobAdvert);
                }
            }

            return false;
        }

        /// <summary>
        /// Fills private lists with data from the database.
        /// </summary>
        /// <returns></returns>
        private async Task LoadDbData()
        {
            _companies = await _companyService.GetAllAsync();
            _categories = await _categoryService.GetCategoriesAsync();
            _specializations = await _categoryService.GetSpecializationsAsync();
            _jobAdverts = await GetRawJobAdvertData();
            _jobAdvertLinks = await GetJobAdvertLinks();
        }

        /// <summary>
        /// Needs some refining
        /// Should return a part of the summary
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetSummaryFromJobAdvert(string url)
        {
            _crawler.Url = url;
            var data = await _crawler.GetJobAdvert(By.TagName("span"));
            foreach (var item in data)
            {
                return item.Text;
            }

            return data[0].Text;
        }

        /// <summary>
        /// Used to get the title on a given jobadvert
        /// Make sure the link is a jobadvert
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetTitleFromJobAdvert(string url)
        {
            List<IDynamicSearchFilter> keyWords = await _dynamicSearchFilterService.GetAllAsync();

            string result = "";
            _crawler.Url = url;
            var data = await _crawler.GetJobAdvert(By.TagName("h2"));

            for (int i = 0; i < keyWords.Count; i++)
            {
                Regex regex = new(@$"\b(\w*{keyWords[i].Key.ToLower()}\w*)\b");
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

        /// <summary>
        /// Gets the raw data from the page 
        /// </summary>
        /// <returns></returns>
        public async Task<List<WebData>> GetRawJobAdvertData()
        {
            JobPages = await _jobPageService.GetJobPagesAsync();
            Task<List<WebData>> task = Task<List<WebData>>.Factory.StartNew(() =>
            {
                List<WebData> data = new();
                //-----------------------------------------
                // Make method to get links from db !!
                //-----------------------------------------
                foreach (var jobPage in JobPages)
                {
                    _crawler.Url = jobPage.URL;
                    data.AddRange(_crawler.GetJobAdvert(By.ClassName(GetClassName(GetCompanyNameFromWebSite(jobPage.URL)))).Result);
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
                foreach (var jobPage in JobPages)
                {
                    //sets the url for the crawler to run on
                    _crawler.Url = jobPage.URL;

                    // Adds the list of links found by the crawler 
                    links.AddRange(_crawler.GetJobAdvertLinks().Result);
                }

                return links;
            });

            return await Task.FromResult(await task);
        }

        /// <summary>
        /// Used to get the specialization from the jobadvert
        /// Make sure the link is a jobadvert
        /// </summary>
        /// <param name="jobAdvertLink"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Compare a string to company name 
        /// returns true if there is a match else false
        /// </summary>
        /// <param name="companyString"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        private bool CheckIfStringHasCompanyName(string companyString, CompanyNames companyName)
        {
            if (companyString == companyName.ToString())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks the url for a company name 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Use to get the right keywords for the page 
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public string GetClassName(CompanyNames companyName)
        {
            switch (companyName)
            {
                case CompanyNames.praktikpladsen:
                    return HtmlClassNames[0];

                default:
                    return null;
            }
        }
    }
}