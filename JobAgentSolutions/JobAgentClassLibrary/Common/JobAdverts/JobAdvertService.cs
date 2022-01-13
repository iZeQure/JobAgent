using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using System.Collections.Generic;
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

        /// <summary>
        /// Creates a new JobAdvert in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            IJobAdvert checkupAdvert = await _repository.GetByIdAsync(entity.Id);
            
            if (checkupAdvert is null)
            {
                return await _repository.CreateAsync(entity);
            }

            return checkupAdvert;
        }

        /// <summary>
        /// Returns a list of all JobAdverts in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IJobAdvert>> GetJobAdvertsAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific JobAdvert from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IJobAdvert> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Returns the count of JobAdverts that have the given Category Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            return await _repository.GetJobAdvertCountByCategoryId(id);
        }

        /// <summary>
        /// Returns the count of JobAdverts that have the given Specialization Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            return await _repository.GetJobAdvertCountBySpecializationId(id);
        }

        /// <summary>
        /// Returns the count of JobAdverts that have no categories
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountByUncategorized()
        {
            return await _repository.GetJobAdvertCountByUncategorized();
        }

        /// <summary>
        /// Removes a JobAdvert from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IJobAdvert entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a JobAdvert in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            return await _repository.UpdateAsync(entity);
        }
    }
}
