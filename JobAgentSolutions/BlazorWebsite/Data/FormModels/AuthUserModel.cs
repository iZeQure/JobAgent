using JobAgentClassLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace BlazorWebsite.Data.FormModels
{
    public class AuthUserModel : BaseModel
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

        public bool IsValidEmail(string Email)
        {
            try
            {
                var addr = new MailAddress(Email);
                return addr.Address == Email;
            }
            catch
            {
                return false;
            }
        }
    }
}
