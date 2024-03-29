using Application.Abstractions.Data;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        return services;
    }
}
