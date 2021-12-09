using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Settings;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace WebCrawler.DataAccess
{
    public class SqlConfigurationSetup
    {
        public static ISqlDbManager SetupSqlDbManager()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                //This is only for developing
                //Release version must NOT have connectionstring data in code
                {"ServerHost", "172.18.3.151" },
                {"Database", "JobAgentDB_v2" }
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
