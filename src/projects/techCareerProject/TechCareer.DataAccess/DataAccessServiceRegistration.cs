using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TechCareer.DataAccess.Contexts;
using TechCareer.DataAccess.Repositories.Abstracts;
using TechCareer.DataAccess.Repositories.Concretes;

namespace TechCareer.DataAccess;

public static class DataAccessServiceRegistration
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services,IConfiguration configuration)
    {

        services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        //Case1
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IInstructorRepository, InstructorRepository>();
        services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
        services.AddScoped<IVideoEducationRepository, VideoEducationRepository>();
        //Case2
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IDictionaryRepository, DictionaryRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ITypOfWorkRepository, TypOfWorkRepository>();
        services.AddScoped<IWorkPlaceRepository, WorkPlaceRepository>();
        services.AddScoped<IYearsOfExperienceRepository, YearsOfExperienceRepository>();



        services.AddDbContext<BaseDbContext>(opt =>
        {
            opt.UseSqlServer(configuration.GetConnectionString("SqlConnection"));
        }, ServiceLifetime.Scoped);

        return services;
    }
}