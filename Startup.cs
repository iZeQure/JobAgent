using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JobAgent.Services;
using JobAgent.Data;
using JobAgent.Data.DataAccess;
using JobAgent.Data.Security;
using BlazorStrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using JobAgent.Data.Interfaces;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Syncfusion.Blazor;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Localization;

namespace JobAgent
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
            DataAccessOptions.ConnectionString = Configuration.GetConnectionString("HomeDB");

            services.AddSingleton<SqlDataAccess>(); // Database Service.
            services.AddTransient<SecurityService>(); // Security Service.
            services.AddTransient<JobService>(); // Job Service.
            services.AddTransient<DataService>(); // Data Service.
            services.AddTransient<AdminService>(); // Admin Service.    

            services.AddRazorPages();
            services.AddBlazoredLocalStorage();
            services.AddServerSideBlazor().AddHubOptions(o => { o.MaximumReceiveMessageSize = 102400000; });
            services.AddBootstrapCss();
            //services.AddSyncfusionBlazor(true);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                // define the list of cultures your app will support
                var supportedCultures = new List<CultureInfo>()
                {
                    new CultureInfo("da")
                };
                // set the default culture
                options.DefaultRequestCulture = new RequestCulture("da");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.RequestCultureProviders = new List<IRequestCultureProvider>() {
                 new QueryStringRequestCultureProvider() // Here, You can also use other localization provider
                };
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileUpload, FileService>();
            services.AddScoped<IRefresh, RefreshService>();
            services.AddScoped<MyAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(provider =>
                provider.GetRequiredService<MyAuthStateProvider>());                  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //Register Syncfusion license
            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA5ODQ4QDMxMzgyZTMyMmUzME4yRnNCVk9POTVFMW8vbElhWThWaEtzY2thUlB6emlQWGtaYTFMZ1Y5Nkk9");

            app.UseRequestLocalization();

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
