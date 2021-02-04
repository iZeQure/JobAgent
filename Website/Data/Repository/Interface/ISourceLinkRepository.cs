using JobAgent.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface ISourceLinkRepository<T> where T : BaseEntity
    {
        Task<bool> Create(T obj);
        Task<bool> Update(T obj);
        Task<bool> Remove(int id);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
    }
}
