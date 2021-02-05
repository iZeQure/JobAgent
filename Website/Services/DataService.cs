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

        public async Task<int> GetCountOfJobAdvertsByCategory(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsByCategoryId(id);
        }

        public async Task<int> GetCountOfJobAdvertsBySpecializationId(int id)
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsBySpecializationId(id);
        }

        public async Task<int> GetCountOfJobAdvertsUncategorized()
        {
            return await DataAccessManager.JobAdvertDataAccessManager().GetCountOfJobAdvertsUncategorized();
        }

        public async Task<IEnumerable<ConsultantArea>> GetAllConsultantAreasTask()
        {
            return await DataAccessManager.ConsultantAreaDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Location>> GetAllLocationsTask()
        {
            return await DataAccessManager.LocationDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await DataAccessManager.CategoryDataAccessManager().GetAllCategories();
        }

        public async Task<IEnumerable<Specialization>> GetAllSpecializations()
        {
            return await DataAccessManager.CategoryDataAccessManager().GetAllSpecializations();
        }

        public async Task<IEnumerable<Company>> GetAllCompanies()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<Company>> GetCompaniesWithContract()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetCompaniesWithContract();
        }

        public async Task<IEnumerable<Company>> GetCompaniesWithOutContract()
        {
            return await DataAccessManager.CompanyDataAccessManager().GetCompaniesWithOutContract();
        }

        public async Task<Company> GetCompanyById(int id)
        {
            return await DataAccessManager.CompanyDataAccessManager().GetById(id);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await DataAccessManager.UserDataAccessManager().GetAll();
        }

        public async Task<IEnumerable<SourceLink>> GetAllSourceLinksAsync()
        {
            return await DataAccessManager.SourceLinkDataAccessManager().GetAll();
        }

        public async Task<SourceLink> GetSourceLinkById(int id)
        {
            return await DataAccessManager.SourceLinkDataAccessManager().GetById(id);
        }

        public static string TruncateString(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + " ...";
        }
    }
}
