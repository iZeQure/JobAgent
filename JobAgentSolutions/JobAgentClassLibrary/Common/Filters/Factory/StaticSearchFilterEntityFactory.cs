using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Filters.Factory
{
    public class StaticSearchFilterEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(StaticSearchFilter) => new StaticSearchFilter
            {
                Id = ParseEntityValueToInt(entityValues[0]),
                FilterType = new() { Id = ParseEntityValueToInt(entityValues[1]) },
                Key = ParseEntityValueToString(entityValues[2])
            },
            nameof(StaticSearchFilterInformation) => new StaticSearchFilterInformation
            {
                StaticSearchFilterId = ParseEntityValueToInt(entityValues[0]),
                FilterTypeId = ParseEntityValueToInt(entityValues[1]),
                StaticSearchFilterKey = ParseEntityValueToString(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };

    }
}
