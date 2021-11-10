using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class StaticSearchFilterService : IStaticSearchFilterService
    {
        private readonly IStaticSearchFilterRepository _repository;

        public StaticSearchFilterService(IStaticSearchFilterRepository repository)
        {
            _repository = repository;
        }

        public async Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IStaticSearchFilter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
