using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using JobAgent.Models;
using Pocos;

namespace JobAgent.Services
{
    public class JobService
    {
        private DataAccessManager DataAccessManager { get; } = new DataAccessManager();

        public async Task<JobAdvertPaginationModel> JobAdvertPagination(int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = 25,
                JobAdverts = (await DataAccessManager.JobAdvertDataAccessManager().GetAll()).OrderBy(x => x.JobRegisteredDate),
                CurrentPage = page
            };

            return paginationModel;
        }

        public async Task<JobAdvertPaginationModel> JobAdvertPagination(int perPage, int page = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdvertsPerPage = perPage,
                JobAdverts = (await DataAccessManager.JobAdvertDataAccessManager().GetAll()).OrderBy(x => x.JobRegisteredDate),
                CurrentPage = page
            };

            return paginationModel;
        }

        public async Task<JobAdvertPaginationModel> FilterJobAdvertPagination(int sortedByCategoryId, int pageNumber = 1)
        {
            var paginationModel = new JobAdvertPaginationModel
            {
                JobAdverts = (await DataAccessManager.JobAdvertDataAccessManager().GetAll()).Where(c => c.Category.Identifier == sortedByCategoryId).OrderBy(x => x.Identifier),
                CurrentPage = pageNumber,
                JobAdvertsPerPage = 25
            };

            return paginationModel;
        }

        public async Task<JobAdvert> GetJobVacancyById(int advertId)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetById(advertId);
        }

        public async Task<List<Category>> GetJobMenuAsync()
        {
            return await DataAccessManager.CategoryDataAccessManager().GetAllCategoriesWithSpecializations();
        }

        public async Task<List<JobAdvert>> GetUncategorizedJobVacancies()
        {
            return (await DataAccessManager.JobAdvertDataAccessManager().GetAll()).Where(x => x.Category.Identifier == 0).ToList();
        }

        public async Task<IEnumerable<JobAdvert>> GetJobVacanciesAsync(int id)
        {
            return (await DataAccessManager.JobAdvertDataAccessManager().GetAllJobAdvertsSortedByCategoryId(id));
        }

        public async Task<IEnumerable<JobAdvert>> GetJobSpecialVacanciesAsync(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetAllJobAdvertsSortedBySpecializationId(id);
        }
    }
}
