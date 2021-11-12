using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using WebCrawler.Crawler;

namespace SkpJobCrawler.Crawler
{
    public class JobCrawler : ICrawler
    {
        private readonly LinkProvider linkProvider;
        internal protected List<HtmlDocument> htmlDocumentsVacantJobs = new List<HtmlDocument>();
        internal protected List<HtmlDocument> htmlDocumentsJobPages = new List<HtmlDocument>();
        public JobCrawler(LinkProvider linkProvider)
        {
            this.linkProvider = linkProvider;
        }

        public void GetDataVacantJobs()
        {
            foreach (var item in linkProvider.vacantJobs)
            {
                HttpClient client = new HttpClient();
                HtmlDocument htmlDocument = new HtmlDocument();
                var data = client.GetStringAsync(item.URL).Result;
                htmlDocument.LoadHtml(data);
                htmlDocumentsVacantJobs.Add(htmlDocument);
            }   
        }

        public void GetDataJobPages()
        {
            foreach (var item in linkProvider.jobPages)
            {
                HttpClient client = new HttpClient();
                HtmlDocument htmlDocument = new HtmlDocument();
                var data = client.GetStringAsync(item.URL).Result;
                htmlDocument.LoadHtml(data);
                htmlDocumentsJobPages.Add(htmlDocument);
            }
        }
    }
}
