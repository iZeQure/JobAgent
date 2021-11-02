using System;

namespace JobAgentClassLibrary.Common.JobAdverts.Entities
{
    public class JobAdvert : IJobAdvert
    {
        public string Title { get; set; }

        public string Summary { get; set; }

        public DateTime RegistrationDateTime { get; set; }

        public int CategoryId { get; set; }

        public int SpecializationId { get; set; }

        public int VacantJobId { get; set; }

        public int Id { get; set; }
    }
}
