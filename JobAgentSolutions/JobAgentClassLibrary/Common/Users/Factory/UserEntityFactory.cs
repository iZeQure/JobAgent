﻿using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Users.Factory
{
    public class UserEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(User) => new User
            {
                Id = ParseValue<int>(entityValues[0]),
                Role = new Role { Id = ParseValue<int>(entityValues[1]) },
                Location = new Location { Id = ParseValue<int>(entityValues[2])},
                FirstName = ParseValue<string>(entityValues[3]),
                LastName = ParseValue<string>(entityValues[4]),
                Email = ParseValue<string>(entityValues[5]),
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
