using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class RegisterContractModel
    {
        [Required]
        public string ContactPerson { get; set; }

        [Required]
        public string ContractFileName { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public DateTime ExpiryDate { get; set; }

        [Required]
        public int SignedByUser { get; set; }

        [Required]
        public int SignedWithCompany { get; set; }
    }
}
