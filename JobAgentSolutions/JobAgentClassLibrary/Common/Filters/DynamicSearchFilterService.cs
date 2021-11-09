using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class DynamicSearchFilterService : IDynamicSearchFilterService
    {
        private readonly IDynamicSearchFilterRepository _repository;

        public DynamicSearchFilterService(IDynamicSearchFilterRepository repository)
        {
            _repository = repository;
        }


        public async Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IDynamicSearchFilter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IDynamicSearchFilter entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
