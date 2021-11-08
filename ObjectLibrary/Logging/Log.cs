using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Logging
{
    public class Log : BaseEntity
    {
        private LogSeverity _logSeverity;
        private DateTime _createdDateTime;
        private string _createdBy;
        private string _action;
        private string _message;

        public Log(int id, LogSeverity logSeverity, DateTime createdDateTime, string createdBy, string action, string message) : base(id)
        {
            _logSeverity = logSeverity;
            _createdDateTime = createdDateTime;
            _createdBy = createdBy;
            _action = action;
            _message = message;
        }

        public LogSeverity LogSeverity { get { return _logSeverity; } }
        public DateTime CreatedDateTime { get { return _createdDateTime; } }
        public string CreatedBy { get { return _createdBy; } }
        public string Action { get { return _action; } }
        public string Message { get { return _message; } }
    }
}
