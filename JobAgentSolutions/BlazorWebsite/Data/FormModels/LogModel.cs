using JobAgentClassLibrary.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Data.FormModels
{
    public class LogModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }
       
        [Required]
        public string Action { get; set; }
        
        [Required]
        public string CreatedBy { get; set; }
        
        [Required]
        public DateTime CreatedDateTime { get; set; }
        
        [Required]
        public LogSeverity LogSeverity { get; set; }

    }
}
