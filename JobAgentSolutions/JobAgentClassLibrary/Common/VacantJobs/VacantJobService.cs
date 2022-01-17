using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs
{
    public class VacantJobService : IVacantJobService
    {
        private readonly IVacantJobRepository _repository;
        private readonly ILogService _logService;

        public VacantJobService(IVacantJobRepository repository)
        {
            _repository = repository;
        }

        public async Task<IVacantJob> CreateAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create VacantJob", nameof(CreateAsync), nameof(VacantJobService), LogType.SERVICE);
                return null;
            }
        }

        public async Task<List<IVacantJob>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (System.Exception ex)
            {
                await _logService.LogError(ex, "Failed to get all VacantJobs", nameof(GetAllAsync), nameof(VacantJobService), LogType.SERVICE);
                return null;
            }
        }

        public async Task<IVacantJob> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (System.Exception ex)
            {
                await _logService.LogError(ex, "Failed to get VacantJob by id", nameof(GetByIdAsync), nameof(VacantJobService), LogType.SERVICE);
                return null;
            }
        }

        public async Task<bool> RemoveAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (System.Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove VacantJob", nameof(RemoveAsync), nameof(VacantJobService), LogType.SERVICE);
                return false;
            }
        }

        public async Task<IVacantJob> UpdateAsync(IVacantJob entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (System.Exception ex)
            {
                await _logService.LogError(ex, "Failed to update VacantJob", nameof(UpdateAsync), nameof(VacantJobService), LogType.SERVICE);
                return null;
            }
        }

    }
}
