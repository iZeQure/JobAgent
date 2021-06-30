using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorServerWebsite.Data;
using BlazorServerWebsite.Data.Providers;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorServerWebsite.Data.Services.Abstractions;
using BlazorServerWebsite.Data.Services;
using SecurityLibrary.Access;
using SecurityLibrary.Interfaces;
using ObjectLibrary.Common.Configuration;
using SqlDataAccessLibrary.Repositories.Abstractions;
using SqlDataAccessLibrary.Repositories;
using SqlDataAccessLibrary.Database;
using Blazored.LocalStorage;
using SecurityLibrary.Providers;

namespace BlazorServerWebsite
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
            var configurationSettings = Configuration.GetSection("JobAgent").Get<ConfigurationSettings>();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();

            services.AddSingleton<IConfigurationSettings>(configurationSettings);
            services.AddTransient<ISqlDatabase, SqlDatabase>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IAreaRepository, AreaRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();

            services.AddScoped<IFileAccess, ContractFileAccess>();
            services.AddScoped<IAuthenticationAccess, UserAccess>();

            services.AddScoped<IAreaService, AreaService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IContractService, ContractService>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IMessageClearProvider, MessageClearProvider>();
            services.AddScoped<FileAccessProvider>();
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
                app.UseBrowserLink();
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
