using JobAgentClassLibrary.Common.Areas.Entities;
using JobAgentClassLibrary.Common.Areas.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Areas.Factory
{
    /// <summary>
    /// Handles the creation of Entities that implements <see cref="IAggregateRoot"/>.
    /// </summary>
    public class AreaEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Area) => new Area
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1])
            },
            nameof(AreaInformation) => new AreaInformation
            {
                AreaId = ParseValue<int>(entityValues[0]),
                AreaName = ParseValue<string>(entityValues[1]),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
