using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Companies
{
    public class CompanyService : ICompanyService
    {
        private readonly ICompanyRepository _repository;

        public CompanyService(ICompanyRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creates a new Company in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>The created Company</returns>
        public async Task<ICompany> CreateAsync(ICompany entity)
        {
            return await _repository.CreateAsync(entity);
        }

        /// <summary>
        /// returns a list of all Companies in the database
        /// </summary>
        /// <returns></returns>
        public async Task<List<ICompany>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// Returns a specific Company from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ICompany> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Removes a Company from the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(ICompany entity)
        {
            return await _repository.RemoveAsync(entity);
        }

        /// <summary>
        /// Updates a Company in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<ICompany> UpdateAsync(ICompany entity)
        {
            return await _repository.UpdateAsync(entity);
        }

    }
}
