﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class RegisterAccountModel
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
        public int ConsultantAreaId { get; set; }

        [Required]
        public int LocationId { get; set; }
    }
}