﻿
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.VacantJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            services.AddSingleton<IJobPageService, JobPageService>();
            services.AddSingleton<IVacantJobService, VacantJobService>();

        }
    }
}
