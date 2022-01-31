using JobAgentClassLibrary.Core.Entities;
using System;

namespace JobAgentClassLibrary.Loggings.Entities
{
    public class SystemLog : ILog
    {
        public LogSeverity LogSeverity { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public int Id { get; set; }
        public LogType LogType { get; set; }
    }
}
