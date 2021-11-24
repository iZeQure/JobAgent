using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Filters
{
    public class FilterTypeService : IFilterTypeService
    {
        private readonly IFilterTypeRepository _repository;

        public FilterTypeService(IFilterTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<IFilterType> CreateAsync(IFilterType entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<List<IFilterType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IFilterType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<bool> RemoveAsync(IFilterType entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        public async Task<IFilterType> UpdateAsync(IFilterType entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
