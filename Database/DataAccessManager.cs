using DataAccess.Repositories;
using DataAccess.Repositories.Interfaces;
using DataAccess.SqlAccess;

namespace DataAccess
{
    public class DataAccessManager
    {
        public IUserRepository UserDataAccessManager() { return new UserRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public ICategoryRepository CategoryDataAccessManager() { return new CategoryRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); } 
        public ISourceLinkRepository SourceLinkDataAccessManager() { return new SourceLinkRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public ILocationRepository LocationDataAccessManager() { return new LocationRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public IJobAdvertRepository JobAdvertDataAccessManager() { return new JobAdvertRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public IContractRepository ContractDataAccessManager() { return new ContractRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public IConsultantAreaRepository ConsultantAreaDataAccessManager() { return new ConsultantAreaRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
        public ICompanyRepository CompanyDataAccessManager() { return new CompanyRepository(databaseAccess: SqlDatabaseAccess.SqlInstance); }
    }
}
