using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface IJobAdvertRepository : IRepository<JobAdvert>
    {
        Task<IEnumerable<JobAdvert>> GetAllUncategorized(CancellationToken cancellation);
        Task<int> GetJobAdvertCountByCategoryId(int id, CancellationToken cancellation);
        Task<int> GetJobAdvertCountBySpecializationId(int id, CancellationToken cancellation);
        Task<int> GetJobAdvertCountByUncategorized(int id, CancellationToken cancellation);

    }
}
