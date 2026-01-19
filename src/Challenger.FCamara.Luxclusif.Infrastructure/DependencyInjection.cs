using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.ExternalServices.Interfaces;
using Challenger.FCamara.Luxclusif.Infrastructure.ExternalServices;
using Challenger.FCamara.Luxclusif.Infrastructure.Persistences.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Challenger.FCamara.Luxclusif.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SqlServer");

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("SqlServer", "String de conexão não localizada.");

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sqlOptions =>
            {
                sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);

                sqlOptions.CommandTimeout(30);

                sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });

        services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

        // External Services
        services.AddHttpClient<IWmsService, WmsService>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:WmsApiUrl"] ?? "https://api.wms.com");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddHttpClient<IAuditService, AuditService>(client =>
        {
            client.BaseAddress = new Uri(configuration["ExternalServices:AuditApiUrl"] ?? "https://api.auditlog.com");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        services.AddScoped<IEmailService, EmailService>();

        return services;
    }
}