﻿using JobAgent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface IJobAdvertRepository : IRepository<JobAdvert>
    {
        public IEnumerable<JobVacanciesAdminModel> GetJobAdvertsForAdmins();

        public JobVacanciesAdminModel GetJobAdvertDetailsForAdminsById(int id);
    }
}
