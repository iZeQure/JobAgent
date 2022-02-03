using JobAgentClassLibrary.Core.Entities;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class BasicUserModel : BaseModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Adgangskode stemmer ikke overens.")]
        [PasswordPropertyText]
        public string ConfirmPassword { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en Rolle.")]
        public int RoleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en lokation.")]
        public int LocationId { get; set; }

        public int Id { get; set; }
    }
}
