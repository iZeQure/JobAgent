using JobAgentClassLibrary.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class LogModel : BaseModel
    {
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string Action { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDateTime { get; set; }

        [Required]
        public LogSeverity LogSeverity { get; set; }

        [Required]
        public LogType LogType { get; set; }

    }
}
