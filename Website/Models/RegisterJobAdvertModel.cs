using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class RegisterJobAdvertModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        [Required]
        public DateTime DeadlineDate { get; set; }

        [Required]
        public string SourceURL { get; set; }


        [Required]
        public int CompanyId { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int SpecializationId { get; set; }
    }
}
