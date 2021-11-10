using HtmlAgilityPack;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using Microsoft.Extensions.Configuration;
using SkpJobCrawler.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;

namespace SkpJobCrawler.Crawler
{
    public class JobCrawler : ICrawler
    {
        private readonly IConfiguration configuration;
        private List<JobAdvert> VacantJobs = new List<VancantJob>();
        private List<VancantJob> JobPages = new List<VancantJob>();

        public JobCrawler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Get the vacant jobs from db
        /// </summary>
        public void GetVacantJobsData()
        {
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("Default"));
            SqlCommand sqlCommand = new SqlCommand("[dbo].[JA.spGetJobAdverts]", connection);
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                int vacantJob_id = (int)reader["VacantJobId"];
                int Company_Id = (int)reader["CompanyId"];
                string JobPageUrl = (string)reader["JobPageUrl"];

                VancantJob vancantJob = new VancantJob() { Company_Id = Company_Id, Id = vacantJob_id, Url = JobPageUrl };
                VacantJobs.Add(vancantJob);
            }
            Console.WriteLine("Done");
            connection.Close();
        }

        /// <summary>
        /// Gets the job pages from db
        /// </summary>
        public void GetJobPages()
        {
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("Default"));
            SqlCommand sqlCommand = new SqlCommand("[dbo].[JA.spGetJobPages]", connection);
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                int vacantJob_id = (int)reader["VacantJobId"];
                int Company_Id = (int)reader["CompanyId"];
                string JobPageUrl = (string)reader["VacantJobUrl"];

                VancantJob jobPage = new VancantJob() { Company_Id = Company_Id, Id = vacantJob_id, Url = JobPageUrl };
                JobPages.Add(jobPage);
            }

            Console.WriteLine("Done");
            connection.Close();
        }

        public void GetVacantJobs()
        {
            string url = "https://www.dr.dk/tjenester/job-widget/";
            HttpClient client = new HttpClient();
            HtmlDocument htmlDocument = new HtmlDocument();
            var data = client.GetStringAsync(url).Result;
            htmlDocument.LoadHtml(data);
            var nodes = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'dre-teaser-title__text')]");
            //IEnumerable<HtmlNode> nodes = htmlDocument.DocumentNode.Descendants(0).Where(n => n.HasClass("dre-teaser-title"));


            Console.WriteLine(htmlDocument.DocumentNode.InnerHtml);

            //foreach (var item in nodes)
            //{
            //    Console.WriteLine(item.InnerText);
            //}

        }
    }
}
