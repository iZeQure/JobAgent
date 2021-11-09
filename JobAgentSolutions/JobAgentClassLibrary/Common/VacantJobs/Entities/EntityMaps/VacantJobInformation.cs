using Dapper.Contrib.Extensions;


namespace JobAgentClassLibrary.Common.VacantJobs.Entities.EntityMaps
{
    [Table("VacantJobInformation")]
    public class VacantJobInformation : IVacantJob
    {
        public int Id => VacantJobId;

        public string URL => VacantJobUrl;


        [Key]
        public int VacantJobId { get; set; }
        public int CompanyId { get; set; }
        public string VacantJobUrl { get; set; }
    }
}
