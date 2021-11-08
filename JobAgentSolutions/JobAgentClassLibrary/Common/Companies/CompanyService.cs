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


        public async Task<ICompany> CreateAsync(ICompany entity)
        {
            return await _repository.CreateAsync(entity);
        }


        public async Task<List<ICompany>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }


        public async Task<ICompany> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }


        public async Task<bool> RemoveAsync(ICompany entity)
        {
            return await _repository.RemoveAsync(entity);
        }


        public async Task<ICompany> UpdateAsync(ICompany entity)
        {
            return await _repository.UpdateAsync(entity);
        }

    }
}
