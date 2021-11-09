using JobAgentClassLibrary.Common.JobPages.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.JobPages.Factory
{
    public class JobPageEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(JobPage) => new JobPage
            {
                Id = ParseValue<int>(entityValues[0]),
                CompanyId = ParseValue<int>(entityValues[1]),
                URL = ParseValue<string>(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
