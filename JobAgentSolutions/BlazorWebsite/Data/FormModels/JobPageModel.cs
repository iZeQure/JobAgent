using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class JobPageModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CompanyId { get; set; }

        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 1)]
        public string URL { get; set; }
    }
}
