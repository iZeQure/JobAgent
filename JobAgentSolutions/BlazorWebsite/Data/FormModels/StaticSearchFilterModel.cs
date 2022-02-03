using JobAgentClassLibrary.Common.Filters.Entities;
using JobAgentClassLibrary.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace BlazorWebsite.Data.FormModels
{
    public class StaticSearchFilterModel : BaseModel
    {
        [Required]
        public int Id { get; set; }

        public FilterType FilterType { get; set; }

        public string Key { get; set; }
    }
}
