using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.JobPages.Entities
{
    public interface IJobPage : IAggregateRoot, IEntity<int>
    {
        public int CompanyId { get; }
        public string URL { get; }
    }
}
