using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Areas
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _repository;

        public AreaService(IAreaRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Saves a new Area in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The saved object</returns>
        public async Task<IArea> CreateAsync(IArea entity)
        {
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// Returns a list of all areas
        /// </summary>
        /// <returns></returns>
        public async Task<List<IArea>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific area on it's id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IArea> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a specific area from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IArea entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a specific area in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IArea> UpdateAsync(IArea entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
