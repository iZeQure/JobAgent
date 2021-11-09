using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Locations.Factory
{
    public class LocationEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Location) => new Location
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
