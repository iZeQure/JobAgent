using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class AccountModel
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

        [Required]
        public int ConsultantAreaId { get; set; }

        [Required]
        public int LocationId { get; set; }

        public override string ToString()
        {
            return $"Email : {Email, 10}\n FirstName : {FirstName, 10}\n LastName : {LastName, 10}\n Area Id : {ConsultantAreaId, 10}\n Location Id : {LocationId, 10}\n";
        }
    }
}
