using DataAccess.Repositories.Base;
using Pocos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public Task<IEnumerable<Company>> GetCompaniesWithContract();

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract();
    }
}
