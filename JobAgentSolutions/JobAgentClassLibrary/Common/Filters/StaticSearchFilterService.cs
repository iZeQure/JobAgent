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

        /// <summary>
        /// Creates a StaticSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IStaticSearchFilter> CreateAsync(IStaticSearchFilter entity)
        {
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all StaticSearchFilters in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IStaticSearchFilter>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific StaticSearchFilter from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IStaticSearchFilter> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a StaticSearchFilter from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IStaticSearchFilter entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a StaticSearchFilter in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IStaticSearchFilter> UpdateAsync(IStaticSearchFilter entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
