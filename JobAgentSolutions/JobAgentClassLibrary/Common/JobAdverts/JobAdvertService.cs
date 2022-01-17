using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts
{
    public class JobAdvertService : IJobAdvertService
    {
        private readonly IJobAdvertRepository _repository;
        private readonly ILogService _logService;

        public JobAdvertService(IJobAdvertRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new JobAdvert in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created object</returns>
        public async Task<IJobAdvert> CreateAsync(IJobAdvert entity)
        {
            try
            {
                IJobAdvert checkupAdvert = await _repository.GetByIdAsync(entity.Id);

                if (checkupAdvert is null)
                {
                    return await _repository.CreateAsync(entity);
                }

                return checkupAdvert;
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create JobAdvert", nameof(CreateAsync), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a list of all JobAdverts in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IJobAdvert>> GetJobAdvertsAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get JobAdverts", nameof(GetJobAdvertsAsync), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific JobAdvert from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IJobAdvert> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get JobAdvert by id", nameof(GetByIdAsync), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns the count of JobAdverts that have the given Category Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountByCategoryId(int id)
        {
            try
            {
                return await _repository.GetJobAdvertCountByCategoryId(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get number of JobAdverts by category id", nameof(GetJobAdvertCountByCategoryId), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns the count of JobAdverts that have the given Specialization Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountBySpecializationId(int id)
        {
            try
            {
                return await _repository.GetJobAdvertCountBySpecializationId(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get number of JobAdverts by specialization id", nameof(GetJobAdvertCountBySpecializationId), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns the count of JobAdverts that have no categories
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetJobAdvertCountByUncategorized()
        {
            try
            {
                return await _repository.GetJobAdvertCountByUncategorized();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get number of uncategorized JobAdverts", nameof(GetJobAdvertCountByUncategorized), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a JobAdvert from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IJobAdvert entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get remove JobAdvert", nameof(RemoveAsync), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a JobAdvert in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IJobAdvert> UpdateAsync(IJobAdvert entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get update JobAdvert", nameof(UpdateAsync), nameof(JobAdvertService), LogType.SERVICE);
                throw;
            }
        }
    }
}
