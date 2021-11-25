using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Common.Filters.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Filters.Factory
{
    public class FilterTypeEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(FilterType) => new FilterType
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1]),
                Description = ParseValue<string>(entityValues[2] ?? string.Empty)
            },
            nameof(FilterTypeInformation) => new FilterTypeInformation
            {
                FilterTypeId = ParseValue<int>(entityValues[0]),
                FilterTypeName = ParseValue<string>(entityValues[1]),
                FilterTypeDescription = ParseValue<string>(entityValues[2] ?? string.Empty)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
