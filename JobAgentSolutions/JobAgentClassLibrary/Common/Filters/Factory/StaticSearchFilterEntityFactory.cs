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
                Id = ParseValue<int>(entityValues[0]),
                FilterType = new() { Id = ParseValue<int>(entityValues[1]) },
                Key = ParseValue<string>(entityValues[2])
            },
            nameof(StaticSearchFilterInformation) => new StaticSearchFilterInformation
            {
                StaticSearchFilterId = ParseValue<int>(entityValues[0]),
                FilterTypeId = ParseValue<int>(entityValues[1]),
                StaticSearchFilterKey = ParseValue<string>(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };

    }
}
