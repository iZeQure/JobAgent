using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class CompanyModel
    {
        [Required]
        public int CVR { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string URL { get; set; }
    }
}
