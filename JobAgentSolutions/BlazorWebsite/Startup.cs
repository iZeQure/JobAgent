using Blazored.LocalStorage;
using BlazorWebsite.Data.Providers;
using JobAgentClassLibrary.Common.Areas;
using JobAgentClassLibrary.Common.Areas.Repositories;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Repositories;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Repositories;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Repositories;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Repositories;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using JobAgentClassLibrary.Core.Settings;
using JobAgentClassLibrary.Security.Access;
using JobAgentClassLibrary.Security.interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BlazorWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionSettings = Configuration.GetSection("JobAgent").Get<DbConnectionSettings>();
            var securitySettings = Configuration.GetSection("JobAgent").Get<AppSecuritySettings>();


            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();


            /* Settings Injections */
            services.AddSingleton<IConnectionSettings>(connectionSettings);
            services.AddSingleton<ISecuritySettings>(securitySettings);


            /* Repository Injections */
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IJobAdvertRepository, JobAdvertRepository>();
            services.AddScoped<IJobPageRepository, JobPageRepository>();
            services.AddScoped<IVacantJobRepository, VacantJobRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();


            /* Access Injections */
            services.AddScoped<IAuthenticationAccess, UserAccess>();


            /* Service Injections */
            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IJobAdvertService, JobAdvertService>();
            services.AddScoped<IVacantJobService, VacantJobService>();
            services.AddScoped<IJobPageService, JobPageService>();


            /* Provider Injections */
            services.AddScoped<IMessageClearProvider, MessageClearProvider>();
            services.AddScoped<IRefreshProvider, RefreshProvider>();
            services.AddScoped<MyAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<MyAuthStateProvider>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
