using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class DynamicSearchFilterModel
    {
        [Required]
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public int Specializationid { get; set; }

        [Required]
        public string Key { get; set; }
    }
}
