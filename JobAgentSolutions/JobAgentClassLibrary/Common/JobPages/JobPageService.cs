using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobPages
{
    public class JobPageService : IJobPageService
    {
        private readonly IJobPageRepository _jobPageRepository;
        private readonly ILogService _logService;

        public JobPageService(IJobPageRepository jobPageRepository, ILogService logService)
        {
            _jobPageRepository = jobPageRepository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new JobPage in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>the created object</returns>
        public async Task<IJobPage> CreateAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create JobPage", nameof(CreateAsync), nameof(JobPageService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific JobPage from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IJobPage> GetByIdAsync(int id)
        {
            try
            {
                return await _jobPageRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get JobPage by id", nameof(GetByIdAsync), nameof(JobPageService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns all JobPages in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<IJobPage>> GetJobPagesAsync()
        {
            try
            {
                return await _jobPageRepository.GetAllSystemLogsAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get JobPages", nameof(GetJobPagesAsync), nameof(JobPageService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a JobPage from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove JobPage", nameof(RemoveAsync), nameof(JobPageService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a JobPage in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<IJobPage> UpdateAsync(IJobPage entity)
        {
            try
            {
                return await _jobPageRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update JobPage", nameof(UpdateAsync), nameof(JobPageService), LogType.SERVICE);
                throw;
            }
        }
    }
}
