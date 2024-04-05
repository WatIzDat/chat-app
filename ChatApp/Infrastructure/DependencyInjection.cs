using Application.Abstractions.Data;
using Infrastructure.BackgroundJobs;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("PostgresDatabase")
            ?? throw new Exception("Connection string 'PostgresDatabase' was not found.");

        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.MigrationsAssembly("Infrastructure"));
        });

        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<UnbanUserBackgroundJobSetup>();

        return services;
    }
}
