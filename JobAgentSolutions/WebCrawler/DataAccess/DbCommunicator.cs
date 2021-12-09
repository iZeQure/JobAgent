using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Common.Categories.Factory;
using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Factory;
using JobAgentClassLibrary.Common.Companies.Repositories;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Factory;
using JobAgentClassLibrary.Common.Filters.Repositories;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Factory;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebCrawler.DataAccess
{
    public class DbCommunicator
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISpecializationRepository _specializationRepository;
        private readonly ICategoryService _categoryService;
        private readonly IDynamicSearchFilterRepository _dynamicSearchFilterRepository;
        private readonly IDynamicSearchFilterService _dynamicSearchFilterService;
        private readonly IStaticSearchFilterRepository _staticSearchFilterRepository;
        private readonly IStaticSearchFilterService _staticSearchFilterService;
        private readonly IJobPageRepository _jobPageRepository;
        private readonly IJobPageService _jobPageService;
        private readonly IJobAdvertRepository _jobAdvertRepository;
        private readonly IJobAdvertService _jobAdvertService;
        private readonly IVacantJobRepository _vacantJobRepository;
        private readonly IVacantJobService _vacantJobService;
        private readonly ICompanyRepository _companyRepository;
        private readonly ICompanyService _companyService;

        public DbCommunicator()
        {
            var manager = SqlConfigurationSetup.SetupSqlDbManager();
            var categoryFactory = new CategoryEntityFactory();
            var dynamicSearchFilterFactory = new DynamicSearchFilterEntityFactory();
            var staticSearchFilterFactory = new StaticSearchFilterEntityFactory();
            var jobPageFactory = new JobPageEntityFactory();
            var jobAdvertFactory = new JobAdvertEntityFactory();
            var vacantJobFactory = new VacantJobEntityFactory();
            var companyFactory = new CompanyEntityFactory();

            _categoryRepository = new CategoryRepository(manager, categoryFactory);
            _specializationRepository = new SpecializationRepository(manager, categoryFactory);
            _categoryService = new CategoryService(_categoryRepository, _specializationRepository);

            _dynamicSearchFilterRepository = new DynamicSearchFilterRepository(manager, dynamicSearchFilterFactory);
            _dynamicSearchFilterService = new DynamicSearchFilterService(_dynamicSearchFilterRepository);

            _staticSearchFilterRepository = new StaticSearchFilterRepository(manager, staticSearchFilterFactory);
            _staticSearchFilterService = new StaticSearchFilterService(_staticSearchFilterRepository);

            _jobPageRepository = new JobPageRepository(manager, jobPageFactory);
            _jobPageService = new JobPageService(_jobPageRepository);

            _jobAdvertRepository = new JobAdvertRepository(manager, jobAdvertFactory);
            _jobAdvertService = new JobAdvertService(_jobAdvertRepository);

            _vacantJobRepository = new VacantJobRepository(manager, vacantJobFactory);
            _vacantJobService = new VacantJobService(_vacantJobRepository);

            _companyRepository = new CompanyRepository(manager, companyFactory);
            _companyService = new CompanyService(_companyRepository);
        }

        public async Task<IEnumerable<ICategory>> GetCategoriesAsync()
        {
            return await _categoryService.GetCategoriesAsync();
        }

        public async Task<List<ISpecialization>> GetSpecializationsAsync()
        {
            return await _categoryService.GetSpecializationsAsync();
        }

        public async Task<List<IDynamicSearchFilter>> GetDynamicSearchFiltersAsync()
        {
            return await _dynamicSearchFilterService.GetAllAsync();
        }

        public async Task<List<IStaticSearchFilter>> GetStaticSearchFiltersAsync()
        {
            return await _staticSearchFilterService.GetAllAsync();
        }

        public async Task<List<IJobPage>> GetJobPagesAsync()
        {
            return await _jobPageService.GetJobPagesAsync();
        }

        public async Task<List<IVacantJob>> GetVacantJobsAsync()
        {
            return await _vacantJobService.GetAllAsync();
        }

        public async Task<List<IJobAdvert>> GetJobAdvertsAsync()
        {
            return await _jobAdvertService.GetJobAdvertsAsync();
        }

        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            return await _jobAdvertService.CreateAsync(entity);
        }

        public async Task<IJobAdvert> GetJobAdvertByIdAsync(int id)
        {
            return await _jobAdvertService.GetByIdAsync(id);
        }

        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            return await _jobAdvertService.UpdateAsync(entity);
        }

        public async Task<IVacantJob> CreateVacantJobAsync(IVacantJob entity)
        {
            return await _vacantJobService.CreateAsync(entity);
        }

        public async Task<IVacantJob> GetVacantJobByIdAsync(int id)
        {
            return await _vacantJobService.GetByIdAsync(id);
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            return await _vacantJobService.UpdateAsync(entity);
        }

        public async Task<List<ICompany>> GetCompaniesAsync()
        {
            return await _companyService.GetAllAsync();
        }
    }
}
