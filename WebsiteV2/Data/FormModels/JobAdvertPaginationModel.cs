using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.FormModels
{
    public class JobAdvertPaginationModel
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
