using Business.Abstract;
using Business.Concrete;
using Business.Utils.TokenService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Business;

public static class ServiceRegistration
{
    public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddSingleton<ITokenService, TokenService>();
        services.AddScoped<IAuthService, AuthService>();

        #region ENTITY SERVICES
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IBlogLikeService, BlogLikeService>();
        services.AddScoped<IBlogCommentService, BlogCommentService>();
        services.AddScoped<ICategoryService, CategoryService>();
        #endregion

        return services;
    }
}
