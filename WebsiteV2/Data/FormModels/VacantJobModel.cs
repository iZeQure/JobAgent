using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.FormModels
{
    public class VacantJobModel
    {
        [Required]
        public int VacantJobId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string URL { get; set; }



    }
}
