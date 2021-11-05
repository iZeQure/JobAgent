using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Filters.Factory
{
    public class DynamicSearchFilterEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(DynamicSearchFilter) => new DynamicSearchFilter
            {
                Id = ParseValue<int>(entityValues[0]),
                SpecializationId = ParseValue<int>(entityValues[1]),
                CategoryId = ParseValue<int>(entityValues[2]),
                Key = ParseValue<string>(entityValues[3])
            },
            nameof(DynamicSearchFilterInformation) => new DynamicSearchFilterInformation
            {
                DynamicSearchFilterId = ParseValue<int>(entityValues[0]),
                SpecializationId = ParseValue<int>(entityValues[1]),
                CategoryId = ParseValue<int>(entityValues[2]),
                DynamicSearchFilterKey = ParseValue<string>(entityValues[3])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")

        };
    }
}
