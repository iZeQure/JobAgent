using Dapper.Contrib.Extensions;
using JobAgentClassLibrary.Core.Entities;
using System;

namespace JobAgentClassLibrary.Loggings.Entities.EntityMaps
{
    [Table("SystemLogInformation")]
    public class LogInformation : ILog
    {
        public DateTime CreatedDateTime => LogCreatedDateTime;

        public string CreatedBy => LogCreatedBy;

        public string Action => LogAction;

        public string Message => LogMessage;

        public int Id => LogId;

        [Key]
        public int LogId { get; set; }
        public string LogCreatedBy { get; set; }
        public DateTime LogCreatedDateTime { get; set; }
        public string LogAction { get; set; }
        public string LogMessage { get; set; }
        public LogSeverity LogSeverity { get; set; }
        public LogType LogType { get; set; }
    }
}
