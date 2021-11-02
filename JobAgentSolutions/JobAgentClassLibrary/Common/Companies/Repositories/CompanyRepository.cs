using JobAgentClassLibrary.Common.Companies.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Companies.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        public Task<ICompany> CreateAsync(ICompany entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<ICompany>> GetAllAsync(ICompany entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICompany> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveAsync(ICompany entity)
        {
            throw new NotImplementedException();
        }

        public Task<ICompany> UpdateAsync(ICompany entity)
        {
            throw new NotImplementedException();
        }
    }
}
