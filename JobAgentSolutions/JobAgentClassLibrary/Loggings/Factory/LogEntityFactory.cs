using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using JobAgentClassLibrary.Loggings.Entities;
using System;

namespace JobAgentClassLibrary.Loggings.Factory
{
    public class LogEntityFactory : FactoryParser, IFactory
    {
        /// <summary>
        /// Creates new entity.
        /// </summary>
        /// <remarks>
        /// <br>entityValues in order:</br>
        /// <br> Id as <see cref="int"/> default = 0, </br> <br> LogSeverity as <see cref="LogSeverity"/>,</br> <br> Message as <see cref="string"/>, </br> <br> Action as <see cref="string"/>, </br> <br> CreatedBy as <see cref="string"/>, </br> <br> CreatedDateTime as <see cref="DateTime"/>, </br> <br> LogType as <see cref="LogType"/> </br>
        /// </remarks>
        /// <param name="paramName">name of the entity type as the string constant.</param>
        /// <param name="entityValues">Defines values to populate an <see cref="ILog"/> entity.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(DbLog) => new DbLog
            {
                Id = ParseValue<int>(entityValues[0]),
                LogSeverity = ParseValue<LogSeverity>(entityValues[1]),
                Message = ParseValue<string>(entityValues[2]),
                Action = ParseValue<string>(entityValues[3]),
                CreatedBy = ParseValue<string>(entityValues[4]),
                CreatedDateTime = ParseValue<DateTime>(entityValues[5]),
                LogType = ParseValue<LogType>(entityValues[6])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Couldn't create type. Out of range.")
        };
    }
}
