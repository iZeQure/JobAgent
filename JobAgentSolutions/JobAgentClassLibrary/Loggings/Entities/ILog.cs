using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;
using System;

namespace JobAgentClassLibrary.Loggings.Entities
{
    public interface ILog : IAggregateRoot, IEntity<int>
    {
        public LogSeverity LogSeverity { get; }
        public DateTime CreatedDateTime { get; }
        public string CreatedBy { get; }
        public string Action { get ;}
        public string Message { get ; }
        public LogType LogType { get; set; }
    }
}
