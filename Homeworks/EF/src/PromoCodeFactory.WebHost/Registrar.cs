using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.DataAccess.Context;
using PromoCodeFactory.DataAccess.Data;
using PromoCodeFactory.DataAccess.Repositories;
using PromoCodeFactory.WebHost.Settings;

namespace PromoCodeFactory.WebHost
{
    /// <summary>
    /// Регистратор сервиса.
    /// </summary>
    public static class Registrar
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = configuration.Get<ApplicationSettings>();
            services.AddSingleton(applicationSettings)
                    .AddSingleton((IConfigurationRoot)configuration)
                    .ConfigureContext(applicationSettings.ConnectionString)
                    .InstallRepositories(configuration);
            return services;
        }

        private static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection
                .AddTransient<IRepository<Employee>, EmployeeRepository>()
                .AddTransient<IRepository<Role>, RoleRepository>();
                // .AddScoped(typeof(IRepository<Employee>), (x) =>
                // new EmployeeRepository(new DatabaseContext(
                //     new DbContextOptionsBuilder<DatabaseContext>()
                //         .UseSqlite(configuration.Get<ApplicationSettings>().ConnectionString).Options)));
            return serviceCollection;
        }
    }
}
