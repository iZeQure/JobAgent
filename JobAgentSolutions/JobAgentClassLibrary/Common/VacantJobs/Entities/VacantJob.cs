namespace JobAgentClassLibrary.Common.VacantJobs.Entities
{
    public class VacantJob : IVacantJob
    {
        public int CompanyId { get; set; }

        public string URL { get; set; }

        public int Id { get; set; }
    }
}
