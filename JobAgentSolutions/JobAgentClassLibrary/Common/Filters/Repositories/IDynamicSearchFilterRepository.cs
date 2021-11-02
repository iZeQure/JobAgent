using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Filters.Repositories
{
    public interface IDynamicSearchFilterRepository : IRepository<IDynamicSearchFilter, int>
    {
    }
}
