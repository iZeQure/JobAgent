using JobAgentClassLibrary.Common.Companies.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Companies
{
    public interface ICompanyService
    {
        Task<ICompany> CreateAsync(ICompany entity);
        Task<List<ICompany>> GetAllAsync();
        Task<ICompany> GetByIdAsync(int id);
        Task<bool> RemoveAsync(ICompany entity);
        Task<ICompany> UpdateAsync(ICompany entity);


    }
}
