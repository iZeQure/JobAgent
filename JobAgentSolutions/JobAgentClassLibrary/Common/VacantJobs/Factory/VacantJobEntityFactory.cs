using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.VacantJobs.Factory
{
    public class VacantJobEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(VacantJob) => new VacantJob
            {
                Id = ParseValue<int>(entityValues[0]),
                CompanyId = ParseValue<int>(entityValues[1]),
                URL = ParseValue<string>(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
