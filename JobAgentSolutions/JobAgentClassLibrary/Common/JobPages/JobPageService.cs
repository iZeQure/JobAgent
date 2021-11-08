using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages
{
    public class JobPageService : IJobPageService
    {
        private readonly IJobPageRepository _jobPageRepository;

        public JobPageService(IJobPageRepository jobPageRepository)
        {
            _jobPageRepository = jobPageRepository;
        }

        public async Task<IJobPage> CreateAsync(IJobPage entity)
        {
            return await _jobPageRepository.CreateAsync(entity);
        }

        public async Task<IJobPage> GetByIdAsync(int id)
        {
            return await _jobPageRepository.GetByIdAsync(id);
        }

        public async Task<List<IJobPage>> GetJobPagesAsync()
        {
            return await _jobPageRepository.GetAllAsync();
        }

        public async Task<bool> RemoveAsync(IJobPage entity)
        {
            return await _jobPageRepository.RemoveAsync(entity);
        }

        public async Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            return await _jobPageRepository.UpdateAsync(entity);
        }
    }
}
