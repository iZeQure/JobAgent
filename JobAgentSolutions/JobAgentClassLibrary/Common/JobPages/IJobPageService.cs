using JobAgentClassLibrary.Common.JobPages.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages
{
    public interface IJobPageService
    {
        Task<IJobPage> CreateAsync(IJobPage entity);
        Task<List<IJobPage>> GetJobPagesAsync();
        Task<IJobPage> GetByIdAsync(int id);
        Task<bool> RemoveAsync(IJobPage entity);
        Task<IJobPage> UpdateAsync(IJobPage entity);
    }
}
