using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace SqlDataAccessLibrary.Repositories.Abstractions
{
    public interface IRepository<TBaseEntity> where TBaseEntity : IBaseEntity
    {
        Task<int> CreateAsync(TBaseEntity createEntity, CancellationToken cancellation);

        Task<int> DeleteAsync(TBaseEntity deleteEntity, CancellationToken cancellation);

        Task<int> UpdateAsync(TBaseEntity updateEntity, CancellationToken cancellation);

        Task<TBaseEntity> GetByIdAsync(int id, CancellationToken cancellation);

        Task<IEnumerable<TBaseEntity>> GetAllAsync(CancellationToken cancellation);
    }
}
