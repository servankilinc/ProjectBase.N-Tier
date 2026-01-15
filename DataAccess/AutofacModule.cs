using Autofac;
using Autofac.Extras.DynamicProxy;
using Core.Utils.CrossCuttingConcerns;
using DataAccess.Abstract;
using DataAccess.Concrete;

namespace DataAccess;

public class AutofacModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Repositoy Services
        builder.RegisterType<UserRepository>().As<IUserRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogRepository>().As<IBlogRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<CategoryRepository>().As<ICategoryRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogLikeRepository>().As<IBlogLikeRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<BlogCommentRepository>().As<IBlogCommentRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<RefreshTokenRepository>().As<IRefreshTokenRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();
                    
        builder.RegisterType<LanguageRepository>().As<ILanguageRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<LocalizationRepository>().As<ILocalizationRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();

        builder.RegisterType<LocalizationLanguageDetailRepository>().As<ILocalizationLanguageDetailRepository>()
            .EnableInterfaceInterceptors()
            .InterceptedBy(typeof(DataAccessExceptionHandlerInterceptor))
            .InstancePerLifetimeScope();
    }
}