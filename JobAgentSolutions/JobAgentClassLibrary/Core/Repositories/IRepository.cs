using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Core.Repositories
{
    /// <summary>
    /// Interface to represent a repository with CRUD operations.
    /// </summary>
    /// <typeparam name="TAggregateType">Type of the data to aggregate.</typeparam>
    /// <typeparam name="KCriteria"></typeparam>
    public interface IRepository<TAggregateType, KCriteria> where TAggregateType : IAggregateRoot
    {
        Task<TAggregateType> CreateAsync(TAggregateType entity);
        Task<TAggregateType> UpdateAsync(TAggregateType entity);
        Task<bool> RemoveAsync(TAggregateType entity);
        Task<TAggregateType> GetByIdAsync(KCriteria id);
        Task<List<TAggregateType>> GetAllAsync();
    }
}
