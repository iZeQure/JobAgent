using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class ContractModel
    {
        private DateTime registrationDateTime = DateTime.UtcNow;
        private DateTime expiryDate;

        public int Id { get; set; }

        [Required]
        public string ContactPerson { get; set; } = string.Empty;

        [Required]
        public string ContractFileName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime RegistrationDate { get { return registrationDateTime; } set { registrationDateTime = value; } }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ExpiryDate { get { return registrationDateTime.AddYears(5); } set { expiryDate = value; } }

        [Required(ErrorMessage = "Vælg venligst en konsulent fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en konsulent fra listen.")]
        public int SignedByUser { get; set; }

        [Required(ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vælg venligst en virksomhed fra listen.")]
        public int SignedWithCompany { get; set; }
    }
}
