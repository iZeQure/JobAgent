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
                Id = ParseEntityValueToInt(entityValues[0]),
                SpecializationId = ParseEntityValueToInt(entityValues[1]),
                CategoryId = ParseEntityValueToInt(entityValues[2]),
                Key = ParseEntityValueToString(entityValues[3])
            },
            nameof(DynamicSearchFilterInformation) => new DynamicSearchFilterInformation
            {
                DynamicSearchFilterId = ParseEntityValueToInt(entityValues[0]),
                SpecializationId = ParseEntityValueToInt(entityValues[1]),
                CategoryId = ParseEntityValueToInt(entityValues[2]),
                DynamicSearchFilterKey = ParseEntityValueToString(entityValues[3])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")

        };
    }
}
