using System.Reflection;
using Core.AOP.Helpers;
using Core.CrossCuttingConcerns.Rules;
using Core.CrossCuttingConcerns.Serilog;
using Core.CrossCuttingConcerns.Serilog.Loggers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

using TechCareer.Service.Abstracts;
using TechCareer.Service.Concretes;


namespace TechCareer.Service;

public static class BusinessServiceRegistration
{
    public static IServiceCollection AddServiceDependencies(this IServiceCollection services)
    { 
    
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserWithTokenService, UserWithTokenService>();
        services.AddScoped<LoggerServiceBase, FileLogger>();
        return services;
    }
    
   

    public static IServiceCollection AddSubClassesOfType(this IServiceCollection services,
        Assembly assembly,
        Type type,
        Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
        return services;
    }
}