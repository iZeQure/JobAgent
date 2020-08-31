using JobAgent.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class JobVacanciesAdminModel
    {
        private JobAdvert jobAdvert;
        private Category category;
        private Specialization specialization;
        private Company company;

        public JobAdvert JobAdvert { get => jobAdvert; set => jobAdvert = value; }
        public Category Category { get => category; set => category = value; }
        public Specialization Specialization { get => specialization; set => specialization = value; }
        public Company Company { get => company; set => company = value; }
    }
}
