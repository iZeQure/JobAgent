﻿using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Settings;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace JobAgentClassLibraryTests.Setup
{
    public static class SqlConfigurationSetup
    {
        public static ISqlDbManager SetupSqlDbManager()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                {"ServerHost", "IGEIRISS\\SQLDEV" },
                {"Database", "JobAgentDevDb" }
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var settings = config.Get<DbConnectionSettings>();

            ISqlDbFactory factory = new SqlDbFactory(settings);
            return new SqlDbManager(factory);
        }
    }
}
