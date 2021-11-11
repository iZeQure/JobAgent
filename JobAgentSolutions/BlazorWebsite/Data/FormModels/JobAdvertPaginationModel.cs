using JobAgentClassLibrary.Common.JobAdverts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorWebsite.Data.FormModels
{
    /// <summary>
    /// Represents a model which handles the pagination of the Job Advert page.
    /// </summary>
    public class JobAdvertPaginationModel
    {
        /// <summary>
        /// Initializes the model with default values.
        /// </summary>
        public JobAdvertPaginationModel(int maxContentPerPage = 25, int currentPage = 1)
        {
            JobAdverts = new();
            JobAdvertsPerPage = maxContentPerPage;
            CurrentPage = currentPage;
        }

        public List<IJobAdvert> JobAdverts { get; set; }
        public int JobAdvertsPerPage { get; set; } = 25;
        public int CurrentPage { get; set; } = 1;
        public IEnumerable<IJobAdvert> PaginatedJobAdverts
        {
            get
            {
                return CreatePagination();
            }
        }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(JobAdverts.Count / (double)JobAdvertsPerPage));
        }

        private IEnumerable<IJobAdvert> CreatePagination()
        {
            int start = (CurrentPage - 1) * JobAdvertsPerPage;

            return JobAdverts
                .OrderBy(j => j.Id)
                .Skip(start)
                .Take(JobAdvertsPerPage);
        }

        public void ResetToDefaultSettings()
        {
            JobAdverts = new();
            JobAdvertsPerPage = 25;
            CurrentPage = 1;
        }
    }
}
