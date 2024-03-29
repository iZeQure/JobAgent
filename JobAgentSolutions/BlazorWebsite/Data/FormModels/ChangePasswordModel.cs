﻿using JobAgentClassLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class ChangePasswordModel : BaseModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(255, ErrorMessage = "Email er for lang (255 karakter begrænse.).")]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255)]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Genindtast din adgangskode.")]
        [StringLength(255)]
        [Compare(nameof(Password), ErrorMessage = "Adgangskode stemmer ikke overens.")]
        public string ConfirmPassword { get; set; }
    }
}
