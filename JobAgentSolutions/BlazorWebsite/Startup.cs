using Blazored.LocalStorage;
using BlazorWebsite.Data.Providers;
using BlazorWebsite.Data.Services;
using JobAgentClassLibrary.Core.Database.Managers;
using JobAgentClassLibrary.Core.Settings;
using JobAgentClassLibrary.Extensions.ServiceCollection;
using JobAgentClassLibrary.Security.Access;
using JobAgentClassLibrary.Security.Cryptography;
using JobAgentClassLibrary.Security.Cryptography.Hashing;
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
            var connectionSettings = Configuration.GetSection("JobAgent:DB").Get<DbConnectionSettings>();
            var securitySettings = Configuration.GetSection("JobAgent:App").Get<AppSecuritySettings>();


            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();


            /* Settings Injections */
            services.AddSingleton<IConnectionSettings>(connectionSettings);
            services.AddSingleton<ISecuritySettings>(securitySettings);


            /* Factory Injections */
            services.AddFactories();


            /* Manager Injections */
            services.AddSingleton<ISqlDbManager, SqlDbManager>();
            services.AddTransient<ICryptographyService, HashingService>();


            /* Repository Injections */
            services.AddRepositories();


            /* Service Injections */
            services.AddEntityServices();
            services.AddScoped<PaginationService>();


            /* Access Injections */
            services.AddScoped<IAuthenticationAccess, UserAccess>();


            /* Provider Injections */
            services.AddTransient<MessageClearProvider>();
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
