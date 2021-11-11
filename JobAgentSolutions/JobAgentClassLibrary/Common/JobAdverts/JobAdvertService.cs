using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts
{
    public class JobAdvertService : IJobAdvertService
    {
        private readonly IJobAdvertRepository _repository;

        public JobAdvertService(IJobAdvertRepository repository)
        {
            _repository = repository;
        }

        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IJobAdvert>> GetJobAdvertsAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IJobAdvert> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            return await _repository.GetJobAdvertCountByCategoryId(id);
        }

        public async Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            return await _repository.GetJobAdvertCountBySpecializationId(id);
        }

        public async Task<int> GetJobAdvertCountByUncategorized()
        {
            return await _repository.GetJobAdvertCountByUncategorized();
        }

        public async Task<bool> RemoveAsync(IJobAdvert entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
