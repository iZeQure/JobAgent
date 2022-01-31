using JobAgentClassLibrary.Common.Areas;
using JobAgentClassLibrary.Common.Areas.Factory;
using JobAgentClassLibrary.Common.Areas.Repositories;
using JobAgentClassLibrary.Common.Categories;
using JobAgentClassLibrary.Common.Categories.Factory;
using JobAgentClassLibrary.Common.Categories.Repositories;
using JobAgentClassLibrary.Common.Companies;
using JobAgentClassLibrary.Common.Companies.Factory;
using JobAgentClassLibrary.Common.Companies.Repositories;
using JobAgentClassLibrary.Common.Filters;
using JobAgentClassLibrary.Common.Filters.Factory;
using JobAgentClassLibrary.Common.Filters.Repositories;
using JobAgentClassLibrary.Common.JobAdverts;
using JobAgentClassLibrary.Common.JobAdverts.Factory;
using JobAgentClassLibrary.Common.JobAdverts.Repositories;
using JobAgentClassLibrary.Common.JobPages;
using JobAgentClassLibrary.Common.JobPages.Factory;
using JobAgentClassLibrary.Common.JobPages.Repositories;
using JobAgentClassLibrary.Common.Locations;
using JobAgentClassLibrary.Common.Locations.Factory;
using JobAgentClassLibrary.Common.Locations.Repositories;
using JobAgentClassLibrary.Common.Roles;
using JobAgentClassLibrary.Common.Roles.Factory;
using JobAgentClassLibrary.Common.Roles.Repositories;
using JobAgentClassLibrary.Common.Users;
using JobAgentClassLibrary.Common.Users.Factory;
using JobAgentClassLibrary.Common.Users.Repositories;
using JobAgentClassLibrary.Common.VacantJobs;
using JobAgentClassLibrary.Common.VacantJobs.Factory;
using JobAgentClassLibrary.Common.VacantJobs.Repositories;
using JobAgentClassLibrary.Core.Database.Factories;
using JobAgentClassLibrary.Loggings;
using JobAgentClassLibrary.Loggings.Factory;
using JobAgentClassLibrary.Loggings.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JobAgentClassLibrary.Extensions.ServiceCollection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityServices(this IServiceCollection services) 
            => services
            .AddScoped<IAreaService, AreaService>()
            .AddScoped<IRoleService, RoleService>()
            .AddScoped<ILocationService, LocationService>()
            .AddScoped<ICompanyService, CompanyService>()
            .AddScoped<ICategoryService, CategoryService>()
            .AddScoped<IDynamicSearchFilterService, DynamicSearchFilterService>()
            .AddScoped<IStaticSearchFilterService, StaticSearchFilterService>()
            .AddScoped<IFilterTypeService, FilterTypeService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IJobAdvertService, JobAdvertService>()
            .AddScoped<IVacantJobService, VacantJobService>()
            .AddScoped<IJobPageService, JobPageService>()
            .AddScoped<ILogService, SystemLogService>();

        public static IServiceCollection AddRepositories(this IServiceCollection services) 
            => services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<ICompanyRepository, CompanyRepository>()
            .AddScoped<ILocationRepository, LocationRepository>()
            .AddScoped<IAreaRepository, AreaRepository>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IJobAdvertRepository, JobAdvertRepository>()
            .AddScoped<IJobPageRepository, JobPageRepository>()
            .AddScoped<IVacantJobRepository, VacantJobRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IDynamicSearchFilterRepository, DynamicSearchFilterRepository>()
            .AddScoped<IStaticSearchFilterRepository, StaticSearchFilterRepository>()
            .AddScoped<IFilterTypeRepository, FilterTypeRepository>()
            .AddScoped<ISpecializationRepository, SpecializationRepository>()
            .AddScoped<ILoggingRepository, SystemLogRepository>();

        public static IServiceCollection AddFactories(this IServiceCollection services)
            => services
                .AddSingleton<ISqlDbFactory, SqlDbFactory>()
                .AddSingleton<AreaEntityFactory>()
                .AddSingleton<CategoryEntityFactory>()
                .AddSingleton<CompanyEntityFactory>()
                .AddSingleton<DynamicSearchFilterEntityFactory>()
                .AddSingleton<FilterTypeEntityFactory>()
                .AddSingleton<StaticSearchFilterEntityFactory>()
                .AddSingleton<JobAdvertEntityFactory>()
                .AddSingleton<JobPageEntityFactory>()
                .AddSingleton<LocationEntityFactory>()
                .AddSingleton<RoleEntityFactory>()
                .AddSingleton<UserEntityFactory>()
                .AddSingleton<VacantJobEntityFactory>()
                .AddSingleton<LogEntityFactory>();

    }
}
