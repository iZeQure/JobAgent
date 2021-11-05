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
                Id = ParseEntityValueToInt(entityValues[0]),
                Name = ParseEntityValueToString(entityValues[1]),
                Description = ParseEntityValueToString(entityValues[2])
            },
            nameof(FilterTypeInformation) => new FilterTypeInformation
            {
                FilterTypeId = ParseEntityValueToInt(entityValues[0]),
                FilterTypeName = ParseEntityValueToString(entityValues[1]),
                FilterTypeDescription = ParseEntityValueToString(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
