using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;
using System.Linq;

namespace JobAgentClassLibrary.Common.Users.Factory
{
    public class UserEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(User) => new User
            {
                Id = ParseValue<int>(entityValues[0]),
                RoleId = ParseValue<int>(entityValues[1]),
                LocationId = ParseValue<int>(entityValues[2]),
                FirstName = ParseValue<string>(entityValues[3]),
                LastName = ParseValue<string>(entityValues[4]),
                Email = ParseValue<string>(entityValues[5])
            },
            nameof(AuthUser) => new AuthUser
            {
                Id = ParseValue<int>(entityValues[0]),
                RoleId = ParseValue<int>(entityValues[1]),
                LocationId = ParseValue<int>(entityValues[2]),
                FirstName = ParseValue<string>(entityValues[3]),
                LastName = ParseValue<string>(entityValues[4]),
                Email = ParseValue<string>(entityValues[5]),
                AccessToken = ParseValue<string>(6)
                //Salt = ParseValue<string>(entityValues.ElementAtOrDefault(7) ?? string.Empty)
                //Password = ParseValue<string>(entityValues.ElementAtOrDefault(8) ?? string.Empty)
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
