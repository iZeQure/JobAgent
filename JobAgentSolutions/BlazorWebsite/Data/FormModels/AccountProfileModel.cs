using System;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class AccountProfileModel
    {
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

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en lokation fra listen.")]
        public int LocationId { get; set; }

        [Required]
        public int RoleId { get; set; }
    }
}
