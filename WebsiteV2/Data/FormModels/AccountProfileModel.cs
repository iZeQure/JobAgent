using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteV2.Data.FormModels
{
    public class AccountProfileModel
    {
        [Required]
        public int AccountId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst fornavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst efternavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Vælg venligst en eller flere områder fra listen.")]
        [MinLength(1, ErrorMessage = "Vælg venligst en eller flere områder fra listen.")]
        public List<int> ConsultantAreaIds { get; set; }

        [Required]
        [Range(1 , 1, ErrorMessage = "Vælg venligst en lokation fra listen.")]
        public int LocationId { get; set; }
    }
}
