using DataAccess.Repositories.Base;
using Pocos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Interfaces
{
    public interface ISourceLinkRepository : IRepository<SourceLink>
    {
        Task<bool> CheckOutput(int input);
    }
}
