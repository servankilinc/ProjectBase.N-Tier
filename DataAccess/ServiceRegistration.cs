using DataAccess.Contexts;
using DataAccess.Interceptors;
using DataAccess.Interceptors.Helpers;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<LocalizationHelper>();

        services.AddSingleton<AuditInterceptor>();
        services.AddSingleton<ArchiveInterceptor>();
        services.AddSingleton<LogInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();
        services.AddSingleton<LocalizationCommandInterceptor>();
        services.AddSingleton<LocalizationQueryInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Database"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<ArchiveInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<LogInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<LocalizationCommandInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<LocalizationQueryInterceptor>());
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
