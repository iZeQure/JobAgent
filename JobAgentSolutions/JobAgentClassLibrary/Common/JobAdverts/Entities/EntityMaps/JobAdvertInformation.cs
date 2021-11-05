using Dapper.Contrib.Extensions;
using System;

namespace JobAgentClassLibrary.Common.JobAdverts.Entities.EntityMaps
{
    [Table("JobAdvertInformation")]
    public class JobAdvertInformation : IJobAdvert
    {
        public string Title => JobAdvertTitle;

        public string Summary => JobAdvertSummary;

        public DateTime RegistrationDateTime => RegistrationDate;

        public int Id => VacantJobId;


        [Key]
        public int VacantJobId { get; set; }
        public string JobAdvertTitle { get; set; }
        public string JobAdvertSummary { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int CategoryId { get; set; }
        public int SpecializationId { get; set; }
    }
}
