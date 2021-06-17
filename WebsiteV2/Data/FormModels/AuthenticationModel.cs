using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebsiteV2.Data.FormModels
{
    public class AuthenticationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email adresse er påkrævet.")]
        [StringLength(255, ErrorMessage = "Email er for long (255 karakter begrænse).")]
        [EmailAddress(ErrorMessage = "Indtast en gyldig email adresse.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Adgangskode er påkrævet.")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
