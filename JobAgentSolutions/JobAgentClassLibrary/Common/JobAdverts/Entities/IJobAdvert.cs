using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.JobAdverts.Entities
{
    public interface IJobAdvert : IAggregateRoot, IEntity<int>
    {
        public string Title { get; }
        public string Summary { get; }
        public DateTime RegistrationDateTime { get; }
        public int CategoryId { get; }
        public int SpecializationId { get; }
        public int VacantJobId { get; }

    }
}
