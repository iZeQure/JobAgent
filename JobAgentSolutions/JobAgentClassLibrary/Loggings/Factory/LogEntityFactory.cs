using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Loggings.Entities;
using System;

namespace JobAgentClassLibrary.Loggings.Factory
{
    public class LogEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(DbLog) => new DbLog
            {
                Id = ParseValue<int>(entityValues[0]),
                LogSeverity = ParseValue<LogSeverity>(entityValues[1]),
                Message = ParseValue<string>(entityValues[2]),
                Action = ParseValue<string>(entityValues[3]),
                CreatedBy = ParseValue<string>(entityValues[4]),
                CreatedDateTime = ParseValue<DateTime>(entityValues[5])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
