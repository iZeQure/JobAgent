using DataAccess;
using Pocos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class DataService
    {
        private DataAccessManager DataAccessManager { get; }

        public DataService()
        {
            DataAccessManager = new DataAccessManager();
        }

        public async Task<int> GetCountOfJobAdvertsByCategoryAsync(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsByCategoryId(id);
        }

        public async Task<int> GetCountOfJobAdvertsBySpecializationIdAsync(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsBySpecializationId(id);
        }

        public async Task<int> GetCountOfJobAdvertsUncategorizedAsync()
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsUncategorized();
        }

        public async Task<IEnumerable<ConsultantArea>> GetAllConsultantAreasAsync()
        {
            return await DataAccessManager.ConsultantAreaDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Location>> GetAllLocationsAsync()
        {
            return await DataAccessManager.LocationDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await DataAccessManager.CategoryDataAccessManager().GetAllCategories();
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializationsAsync()
        {
            return await DataAccessManager.CategoryDataAccessManager().GetAllSpecializations();
        }

        public async Task<IEnumerable<Company>> GetAllCompaniesAsync()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Company>> GetCompaniesWithContractAsync()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetCompaniesWithContract();
        }

        public async Task<IEnumerable<Company>> GetCompaniesWithOutContractAsync()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetCompaniesWithOutContract();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            return await DataAccessManager.CompanyDataAccessManager().GetById(id);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await DataAccessManager.UserDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<SourceLink>> GetAllSourceLinksAsync()
        {
            return await DataAccessManager.SourceLinkDataAccessManager().GetAll();
        }

        public async Task<SourceLink> GetSourceLinkByIdAsync(int id)
        {
            return await DataAccessManager.SourceLinkDataAccessManager().GetById(id);
        }

        public static string TruncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ...";
        }
    }
}
