using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;

namespace DataAccess
{
    public class DataAccessManager
    {
        public IUserRepository UserDataAccessManager() { return new UserRepository(); }
        public ICategoryRepository CategoryDataAccessManager() { return new CategoryRepository(); } 
        public ISourceLinkRepository SourceLinkDataAccessManager() { return new SourceLinkRepository(); }
        public ILocationRepository LocationDataAccessManager() { return new LocationRepository(); }
        public IJobAdvertRepository JobAdvertDataAccessManager() { return new JobAdvertRepository(); }
        public IContractRepository ContractDataAccessManager() { return new ContractRepository(); }
        public IConsultantAreaRepository ConsultantAreaDataAccessManager() { return new ConsultantAreaRepository(); }
        public ICompanyRepository CompanyDataAccessManager() { return new CompanyRepository(); }
    }
}
