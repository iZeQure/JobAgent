using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }

        [Required]
        [Range(10000000, 99999999, ErrorMessage = "Indtast et gyldigt CVR Nummer. (8 siffer)")]
        public int CVR { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Indtast et gyldigt navn.")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Indtast en gyldig URL.")]
        public string URL { get; set; }
    }
}
