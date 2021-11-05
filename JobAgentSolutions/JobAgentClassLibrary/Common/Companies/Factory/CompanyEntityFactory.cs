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
                Id = ParseEntityValueToInt(entityValues[0]),
                Name = ParseEntityValueToString(entityValues[1])
            },
            nameof(CompanyInformation) => new CompanyInformation
            {
                CompanyId = ParseEntityValueToInt(entityValues[0]),
                CompanyName = ParseEntityValueToString(entityValues[1]),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
