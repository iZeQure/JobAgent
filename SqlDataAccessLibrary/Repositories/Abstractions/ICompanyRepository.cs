using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface ICompanyRepository : IRepository<Company>
    {
        public Task<IEnumerable<Company>> GetCompaniesWithContract(CancellationToken cancellation);

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract(CancellationToken cancellation);

    }
}
