using JobAgentClassLibrary.Core.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class RegisterUserModel : BaseModel
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst fornavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Udfyld venligst efternavn.")]
        [StringLength(maximumLength: 128, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255, ErrorMessage = "Adgangskode er for lang eller kort.", MinimumLength = 6)]
        [PasswordPropertyText]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Genindtast din adgangskode.")]
        [Compare(nameof(Password), ErrorMessage = "Adgangskode stemmer ikke overens.")]
        [StringLength(255, ErrorMessage = "Adgangskode er for lang eller kort.", MinimumLength = 6)]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en Rolle.")]
        public int RoleId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en lokation.")]
        public int LocationId { get; set; }
    }
}
