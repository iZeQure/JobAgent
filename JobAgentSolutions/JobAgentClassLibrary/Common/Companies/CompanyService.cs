using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Repositories;
using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Loggings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;
        private readonly ILogService _logService;

        public CompanyService(ICompanyRepository repository, ILogService logService)
        {
            _repository = repository;
            _logService = logService;
        }

        /// <summary>
        /// Creates a new Company in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created Company</returns>
        public async Task<ICompany> CreateAsync(ICompany entity)
        {
            try
            {
                return await _repository.CreateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to create Company", nameof(CreateAsync), nameof(CompanyService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// returns a list of all Companies in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ICompany>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get Companies", nameof(GetAllAsync), nameof(CompanyService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Returns a specific Company from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICompany> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to get Company by id", nameof(GetByIdAsync), nameof(CompanyService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Removes a Company from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ICompany entity)
        {
            try
            {
                return await _repository.RemoveAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to remove Company", nameof(RemoveAsync), nameof(CompanyService), LogType.SERVICE);
                throw;
            }
        }

        /// <summary>
        /// Updates a Company in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ICompany> UpdateAsync(ICompany entity)
        {
            try
            {
                return await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                await _logService.LogError(ex, "Failed to update Company", nameof(UpdateAsync), nameof(CompanyService), LogType.SERVICE);
                throw;
            }
        }

    }
}
