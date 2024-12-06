using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Castle.DynamicProxy;
using Core.AOP.AspectInterceptors;
using Core.CrossCuttingConcerns.Serilog.Loggers;
using Core.CrossCuttingConcerns.Serilog;
using Core.Security.JWT;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;
using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;
using TechCareer.Service.Rules;

namespace TechCareer.Service.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // AutoMapper Kaydı
            builder.Register(ctx => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
            }).CreateMapper()).As<IMapper>().InstancePerLifetimeScope();

            builder.RegisterType<AspectInterceptorSelector>().AsSelf().InstancePerDependency();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();

            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces()
                .EnableInterfaceInterceptors(new ProxyGenerationOptions
                {
                    Selector = new AspectInterceptorSelector()
                }).SingleInstance();

            // TokenHelper Kaydı
            builder.RegisterType<JwtHelper>()
                .As<ITokenHelper>()
                .InstancePerLifetimeScope();


            // Business Rules ve Servis Kayıtları
            builder.RegisterType<UserBusinessRules>().As<IUserBusinessRules>().InstancePerLifetimeScope();
            builder.RegisterType<AuthService>().As<IAuthService>().InstancePerLifetimeScope();
            builder.RegisterType<CategoryBusinessRules>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();
            builder.RegisterType<FileLogger>().As<LoggerServiceBase>().SingleInstance();
        }
    }
}
