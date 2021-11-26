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

        /// <summary>
        /// Creates a new DynamicSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IDynamicSearchFilter> CreateAsync(IDynamicSearchFilter entity)
        {
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all DynamicSearchFilters in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IDynamicSearchFilter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific DynamicSearchFilters from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IDynamicSearchFilter> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a DynamicSearchFilters from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IDynamicSearchFilter entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a DynamicSearchFilters in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IDynamicSearchFilter> UpdateAsync(IDynamicSearchFilter entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
