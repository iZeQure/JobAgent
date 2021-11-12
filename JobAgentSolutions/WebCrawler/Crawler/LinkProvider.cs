using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebCrawler.Crawler
{
    public class LinkProvider
    {
        private readonly IVacantJobService vacantJobService;
        private readonly IJobPageService jobPageService;
        internal protected readonly IConfiguration configuration;
        internal protected List<IJobPage> jobPages = new List<IJobPage>();
        internal protected List<IVacantJob> vacantJobs = new List<IVacantJob>();

        public LinkProvider(IVacantJobService vacantJobService, IJobPageService jobPageService, IConfiguration configuration)
        {
            this.vacantJobService = vacantJobService;
            this.jobPageService = jobPageService;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get the vacant jobs from db
        /// </summary>
        public void GetVacantJobs()
        {
            SqlConnection connection = new SqlConnection(configuration.GetConnectionString("Default"));
            SqlCommand sqlCommand = new SqlCommand("[dbo].[JA.spGetVacantJobs]", connection);
            connection.Open();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                int vacantJob_id = (int)reader["VacantJobId"];
                int Company_Id = (int)reader["CompanyId"];
                string vacantJobUrl = (string)reader["VacantJobUrl"];

                IVacantJob vancantJob = new VacantJob() { Id = vacantJob_id, CompanyId = Company_Id, URL = vacantJobUrl };
                vacantJobs.Add(vancantJob);
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
                int company_Id = (int)reader["CompanyId"];
                string jobPageUrl = (string)reader["VacantJobUrl"];

                IJobPage jobPage = new JobPage() { Id = vacantJob_id, CompanyId = company_Id, URL = jobPageUrl };
                jobPages.Add(jobPage);
            }

            Console.WriteLine("Done");
            connection.Close();
        }
    }
}
