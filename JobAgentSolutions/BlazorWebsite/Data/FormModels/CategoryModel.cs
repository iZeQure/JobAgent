using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class CategoryModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Categoryname { get; set; }

        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }
        public int SpecializationsCategoryId { get; set; }
    }
}
