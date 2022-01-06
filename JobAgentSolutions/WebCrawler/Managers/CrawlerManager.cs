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

        public List<string> KeyWords { get; set; }
        public List<string> Urls { get; set; }
        public List<string> ClassNames { get; set; }
        public CrawlerManager(Crawler crawler)
        {
            _crawler = crawler;
            Urls = new List<string>()
            {
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20programmering",
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/Datatekniker%20med%20speciale%20i%20infrastruktur",
                "https://pms.praktikpladsen.dk/soeg-opslag/0/Data-%20og%20kommunikationsuddannelsen/IT-supporter"
            };
            ClassNames = new List<string>()
            {
                "opslag-container-container"
            };
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


    }
}