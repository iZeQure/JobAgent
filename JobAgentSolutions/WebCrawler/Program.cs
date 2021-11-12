
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SkpJobCrawler.Crawler;
using JobAgentClassLibrary.Common.VacantJobs.Entities;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.VacantJobs;

namespace WebCrawler
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using IHost host = CreateHostBuilder(args).Build();

            var startupService = ActivatorUtilities.GetServiceOrCreateInstance<Startup>(host.Services);

            startupService.StartCrawler();

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
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: true)
              .AddEnvironmentVariables();


        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICrawler, JobCrawler>();
            services.AddSingleton<IJobPageService, JobPageService>();
            services.AddSingleton<IVacantJobService, VacantJobService>();
        }
    }
}
