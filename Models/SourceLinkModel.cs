using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class SourceLinkModel
    {
        [Required(ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Angiv venligst et link.")]
        public string Link { get; set; }
    }
}
