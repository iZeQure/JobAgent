using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Roles.Factory
{
    public class RoleEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Role) => new Role
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1]),
                Description = ParseValue<string>(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
