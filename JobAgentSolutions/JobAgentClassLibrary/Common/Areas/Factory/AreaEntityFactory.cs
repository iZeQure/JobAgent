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
                Id = ParseEntityValueToInt(entityValues[0]),
                Name = ParseEntityValueToString(entityValues[1])
            },
            nameof(AreaInformation) => new AreaInformation
            {
                AreaId = ParseEntityValueToInt(entityValues[0]),
                AreaName = ParseEntityValueToString(entityValues[1]),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
