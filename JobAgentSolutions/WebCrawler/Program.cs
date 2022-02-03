using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Settings;
using JobAgentClassLibrary.Extensions.ServiceCollection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using WebCrawler.DataScrappers;
using WebCrawler.DataScrappers.Drivers;
using WebCrawler.DataSorters;
using WebCrawler.Managers;

namespace WebCrawler
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var startupService = ActivatorUtilities.GetServiceOrCreateInstance<Startup>(host.Services);

            await startupService.StartCrawlerAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder()
               .ConfigureAppConfiguration((hostingContext, configuration) =>
               {
                   BuildConfiguration(hostingContext.HostingEnvironment.EnvironmentName, configuration);
               })
               .ConfigureServices((context, service) =>
               {
                   ConfigureServices(service, context.Configuration);
               });

        private static void BuildConfiguration(string envName, IConfigurationBuilder configuration) =>
          configuration
              //.AddJsonFile("appsettings.json")
              //.AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: true)
              .AddUserSecrets<Program>()
              .AddEnvironmentVariables();


        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionSettings = configuration.GetSection("JobAgent:DB").Get<DbConnectionSettings>();

            services.AddSingleton<IConnectionSettings>(connectionSettings);

            services.AddSingleton<ISqlDbFactory, SqlDbFactory>();
            services.AddSingleton<ISqlDbManager, SqlDbManager>();

            services.AddFactories();


            //------------------- Repositories
            services.AddRepositories();


            //------------------- Services 
            services.AddEntityServices();
            

            //------------------- Crawler
            services.AddSingleton<IWebDriver, ChromeDriver>();
            services.AddSingleton<CrawlerDriver>();
            services.AddSingleton<UrlCutter>();
            services.AddSingleton<CrawlerManager>();
            services.AddSingleton<Crawler>();
        }
    }
}
