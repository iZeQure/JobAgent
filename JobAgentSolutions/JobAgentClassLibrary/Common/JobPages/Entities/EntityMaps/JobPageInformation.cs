using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.JobPages.Entities.EntityMaps
{
    [Table("JobPageInformation")]
    public class JobPageInformation : IJobPage
    {
        public int Id => JobPageId;

        public string URL => JobPageUrl;


        [Key]
        public int JobPageId { get; set; }
        public int CompanyId { get; set; }
        public string JobPageUrl { get; set; }
    }
}
