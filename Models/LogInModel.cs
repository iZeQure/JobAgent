using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class LogInModel
    {
        [Required(
            AllowEmptyStrings = false, 
            ErrorMessage = "Email adresse er påkrævet."
        )]
        [StringLength(
            255, 
            ErrorMessage = "Email er for long (255 karakter begrænse)."
        )]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255)]
        public string Password { get; set; }
    }
}
