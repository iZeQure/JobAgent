using JobAgentClassLibrary.Common.JobAdverts.Entities;
using JobAgentClassLibrary.Common.JobAdverts.Entities.EntityMaps;
using JobAgentClassLibrary.Core.Factories;
using JobAgentClassLibrary.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.JobAdverts.Factory
{
    public class JobAdvertEntityFactory : FactoryParser, IFactory
    {
        public IAggregateRoot CreateEntity(string paramName, params object[] entityValues) => paramName switch
        {
            nameof(JobAdvert) => new JobAdvert
            {
                Id = ParseValue<int>(entityValues[0]),
                CategoryId = ParseValue<int>(entityValues[1]),
                SpecializationId = ParseValue<int>(entityValues[2]),
                Title = ParseValue<string>(entityValues[3]),
                Summary = ParseValue<string>(entityValues[4]),
                RegistrationDateTime = ParseValue<DateTime>(entityValues[5])
            },
            nameof(JobAdvertInformation) => new JobAdvertInformation
            {
                VacantJobId = ParseValue<int>(entityValues[0]),
                CategoryId = ParseValue<int>(entityValues[1]),
                SpecializationId = ParseValue<int>(entityValues[2]),
                JobAdvertTitle = ParseValue<string>(entityValues[3]),
                JobAdvertSummary = ParseValue<string>(entityValues[4]),
                RegistrationDate = ParseValue<DateTime>(entityValues[5])
            },
            _ => throw new ArgumentOutOfRangeException(nameof(paramName), paramName, "Coudln't create type. Out of range.")
        };
    }
}
