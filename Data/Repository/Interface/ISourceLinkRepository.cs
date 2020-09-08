using JobAgent.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface ISourceLinkRepository
    {
        Task<bool> Create(SourceLink obj);
        Task<bool> Update(SourceLink obj);
        Task<bool> Remove(int id);
        Task<SourceLink> GetById(int id);
        Task<IEnumerable<SourceLink>> GetAll();
    }
}
