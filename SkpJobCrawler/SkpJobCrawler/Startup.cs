﻿using Microsoft.Extensions.Logging;
using SkpJobCrawler.Crawler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkpJobCrawler
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly ICrawler crawler;

        public Startup(ILogger<Startup> logger, ICrawler crawler)
        {
            _logger = logger;
            this.crawler = crawler;
        }

        public void Start()
        {
            _logger.LogInformation("Test");
        }

        public void StartCrawler()
        {
            crawler.GetVacantJobs();   
        }


    }
}