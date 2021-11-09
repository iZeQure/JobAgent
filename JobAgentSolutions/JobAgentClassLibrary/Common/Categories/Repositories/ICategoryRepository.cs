using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Categories.Repositories
{
    public interface ICategoryRepository : IRepository<ICategory, int>
    {
    }
}
