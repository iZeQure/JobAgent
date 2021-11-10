﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkpJobCrawler.Crawler
{
    public interface ICrawler
    {
        public void GetVacantJobsData();
        public void GetVacantJobs();
    }
}