using DataAccess.Abstract;
using DataAccess.Concrete;
using DataAccess.Contexts;
using DataAccess.Interceptors;
using DataAccess.UoW;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess;

public static class ServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services, IConfiguration configuration)
    {
        #region REPOSITORY SERVICES
        // dont use repository servicese directly, use unit of work instead. this is just for unit of work to inject repositories.
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBlogLikeRepository, BlogLikeRepository>();
        services.AddScoped<IBlogCommentRepository, BlogCommentRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        #endregion

        #region DB CONTEXT
        services.AddSingleton<AuditInterceptor>();
        services.AddSingleton<ArchiveInterceptor>();
        services.AddSingleton<SoftDeleteInterceptor>();

        services.AddDbContext<AppDbContext>((serviceProvider, opt) =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("Database"))
                .AddInterceptors(serviceProvider.GetRequiredService<AuditInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<ArchiveInterceptor>())
                .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>());
        });
        #endregion

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
