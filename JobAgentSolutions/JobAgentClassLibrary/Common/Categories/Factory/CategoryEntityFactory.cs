using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Factory
{
    public class CategoryEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Category) => new Category
            {
                Id = ParseValue<int>(entityValues[0]),
                Name = ParseValue<string>(entityValues[1]),
            },
            nameof(Specialization) => new Specialization
            {
                Id = ParseValue<int>(entityValues[0]),
                CategoryId = ParseValue<int>(entityValues[1]),
                Name = ParseValue<string>(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
