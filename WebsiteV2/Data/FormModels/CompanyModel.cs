using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerWebsite.Data.FormModels
{
    public class CompanyModel
    {
        [Required]
        public int CompanyId { get; set; }

        [Required]
        [Range(10000000, 99999999, ErrorMessage = "Indtast et gyldigt CVR Nummer. (8 siffer)")]
        public int CVR { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Indtast et gyldigt navn.")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "En Kontakt Person er påkrævet")]
        public string ContactPerson { get; set; }
    }
}
