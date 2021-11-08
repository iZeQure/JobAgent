using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    public interface IBaseService<TBaseEntity>
    {
        Task<int> CreateAsync(TBaseEntity createEntity, CancellationToken cancellation);
        Task<int> DeleteAsync(TBaseEntity deleteEntity, CancellationToken cancellation);
        Task<IEnumerable<TBaseEntity>> GetAllAsync(CancellationToken cancellation);
        Task<TBaseEntity> GetByIdAsync(int id, CancellationToken cancellation);
        Task<int> UpdateAsync(TBaseEntity updateEntity, CancellationToken cancellation);
    }
}