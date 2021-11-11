using JobAgentClassLibrary.Common.VacantJobs.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs
{
    public interface IVacantJobService
    {
        Task<IVacantJob> CreateAsync(IVacantJob entity);
        Task<List<IVacantJob>> GetAllAsync();
        Task<IVacantJob> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IVacantJob entity);
        Task<IVacantJob> UpdateAsync(IVacantJob entity);
    }
}
