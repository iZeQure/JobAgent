using JobAgent.Data.Objects;
using JobAgent.Data.Repository;
using JobAgent.Data.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class JobAdvertPaginationModel
    {
        public IEnumerable<JobAdvert> JobAdverts { get; set; }
        public int JobAdvertsPerPage { get; set; }
        public int CurrentPage { get; set; } = 1;

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(JobAdverts.Count() / (double)JobAdvertsPerPage));
        }

        public IEnumerable<JobAdvert> PaginatedJobAdverts()
        {
            int start = (CurrentPage - 1) * JobAdvertsPerPage;

            return JobAdverts.OrderBy(j => j.Id).Skip(start).Take(JobAdvertsPerPage);
        }
    }
}
