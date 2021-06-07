using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.SqlAccess;

namespace DataAccess
{
    public class DataAccessManager
    {
        private readonly static SqlDatabaseAccess _databaseAccess = SqlDatabaseAccess.SqlInstance;

        public IUserRepository UserDataAccessManager() { return new UserRepository(databaseAccess: _databaseAccess); }
        public ICategoryRepository CategoryDataAccessManager() { return new CategoryRepository(databaseAccess: _databaseAccess); } 
        public ISourceLinkRepository SourceLinkDataAccessManager() { return new SourceLinkRepository(databaseAccess: _databaseAccess); }
        public ILocationRepository LocationDataAccessManager() { return new LocationRepository(databaseAccess: _databaseAccess); }
        public IJobAdvertRepository JobAdvertDataAccessManager() { return new JobAdvertRepository(databaseAccess: _databaseAccess); }
        public IContractRepository ContractDataAccessManager() { return new ContractRepository(databaseAccess: _databaseAccess); }
        public IConsultantAreaRepository ConsultantAreaDataAccessManager() { return new ConsultantAreaRepository(databaseAccess: _databaseAccess); }
        public ICompanyRepository CompanyDataAccessManager() { return new CompanyRepository(databaseAccess: _databaseAccess); }
    }
}
