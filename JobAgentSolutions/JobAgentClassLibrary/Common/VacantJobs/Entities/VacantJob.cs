using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.VacantJobs.Entities
{
    public class VacantJob : IVacantJob
    {
        public int CompanyId { get; set; }

        public string URL { get; set; }

        public int Id { get; set; }
    }
}
