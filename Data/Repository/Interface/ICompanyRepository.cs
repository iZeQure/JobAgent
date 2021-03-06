﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data.Objects;

namespace JobAgent.Data.Repository.Interface
{
    interface ICompanyRepository : IRepository<Company>
    {
        public Task<IEnumerable<Company>> GetCompaniesWithContract();

        public Task<IEnumerable<Company>> GetCompaniesWithOutContract();
    }
}
