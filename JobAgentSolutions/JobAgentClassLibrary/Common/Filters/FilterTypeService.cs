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

        /// <summary>
        /// Creates a new FilterType in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IFilterType> CreateAsync(IFilterType entity)
        {
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all FilterTypes in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IFilterType>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific FilterType from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IFilterType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a FilterType from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IFilterType entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a FilterType in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IFilterType> UpdateAsync(IFilterType entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
