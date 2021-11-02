using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.VacantJobs.Entities
{
    public interface IVacantJob : IAggregateRoot, IEntity<int>
    {
        public int CompanyId { get; }
        public string URL { get; }
    }
}
