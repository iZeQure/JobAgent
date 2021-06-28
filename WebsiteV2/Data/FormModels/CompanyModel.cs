using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlazorServerWebsite.Data.FormModels
{
    public class CompanyModel
    {
        [Required]
        public int CompanyId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CVR er påkrævet")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public int CVR { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Firma Navn er påkrævet")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "En Kontakt Person er påkrævet")]
        public string ContactPerson { get; set; }
    }
}
