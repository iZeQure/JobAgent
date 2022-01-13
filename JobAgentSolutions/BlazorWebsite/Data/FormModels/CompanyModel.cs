using JobAgentClassLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class CompanyModel : BaseModel
    {
        [Required]
        public int CompanyId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Indtast et gyldigt navn.")]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
