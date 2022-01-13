
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Factory;
using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Factory;
using JobAgentClassLibrary.Common.Companies.Repositories;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Factory;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using WebCrawler.DataAccess;
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
              .AddJsonFile("appsettings.json")
              .AddJsonFile($"appsettings.{envName ?? "Production"}.json", optional: true)
              .AddUserSecrets<Program>()
              .AddEnvironmentVariables();


        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var connectionSettings = configuration.GetSection("JobAgent:DB").Get<DbConnectionSettings>();

            services.AddSingleton<IConnectionSettings>(connectionSettings);

            services.AddSingleton<ISqlDbFactory, SqlDbFactory>();
            services.AddSingleton<ISqlDbManager, SqlDbManager>();

            services.AddSingleton<JobAdvertEntityFactory>();
            services.AddSingleton<JobPageEntityFactory>();
            services.AddSingleton<VacantJobEntityFactory>();
            services.AddSingleton<CompanyEntityFactory>();
            services.AddSingleton<CategoryEntityFactory>();

            services.AddScoped<IJobAdvertRepository, JobAdvertRepository>();
            services.AddScoped<IJobPageRepository, JobPageRepository>();
            services.AddScoped<IVacantJobRepository, VacantJobRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ISpecializationRepository, SpecializationRepository>();

            services.AddScoped<IJobAdvertService, JobAdvertService>();
            services.AddScoped<IJobPageService, JobPageService>();
            services.AddScoped<IVacantJobService, VacantJobService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddSingleton<IWebDriver, ChromeDriver>();

            services.AddSingleton<CrawlerDriver>();
            services.AddSingleton<UrlCutter>();
            services.AddSingleton<CrawlerManager>();
            services.AddSingleton<Crawler>();
        }
    }
}
