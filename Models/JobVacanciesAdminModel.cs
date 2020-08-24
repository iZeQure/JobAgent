using JobAgent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class JobVacanciesAdminModel
    {
        private JobAdvert jobAdvert;
        private JobAdvertCategory category;
        private JobAdvertCategorySpecialization specialization;
        private Company company;

        public JobAdvert JobAdvert { get => jobAdvert; set => jobAdvert = value; }
        public JobAdvertCategory Category { get => category; set => category = value; }
        public JobAdvertCategorySpecialization Specialization { get => specialization; set => specialization = value; }
        public Company Company { get => company; set => company = value; }
    }
}
