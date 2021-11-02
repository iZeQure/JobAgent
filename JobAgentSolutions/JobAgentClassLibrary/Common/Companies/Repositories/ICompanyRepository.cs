using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Companies.Repositories
{
    public interface ICompanyRepository : IRepository<ICompany, int>
    {
    }
}
