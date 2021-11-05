using JobAgentClassLibrary.Common.Companies.Entities;
using JobAgentClassLibrary.Common.Companies.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Companies.Factory
{
    public class CompanyEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Company) => new Company
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1])
            },
            nameof(CompanyInformation) => new CompanyInformation
            {
                CompanyId = ParseValue<int>(entityValues[0]),
                CompanyName = ParseValue<string>(entityValues[1]),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
