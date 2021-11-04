using JobAgentClassLibrary.Common.Categories.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Common.Categories.Factory
{
    public class CategoryEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(Category) => new Category
            {
                Id = ParseEntityValueToInt(entityValues[0]),
                Name = ParseEntityValueToString(entityValues[1]),
                Specializations = ParseEntityValueToList<ISpecialization>(entityValues[2]) ?? null
            },
            nameof(Specialization) => new Specialization
            {
                Id = ParseEntityValueToInt(entityValues[0]),
                CategoryId = ParseEntityValueToInt(entityValues[1]),
                Name = ParseEntityValueToString(entityValues[2])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
