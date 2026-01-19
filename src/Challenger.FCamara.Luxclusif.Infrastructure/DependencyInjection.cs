using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
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

        return services;
    }

    //public static IHostApplicationBuilder AddApplication(this IHostApplicationBuilder builder)
    //{
    //    var assembly = typeof(DependencyInjection).Assembly;
    //    builder.AddValidationsBusinessRule(assembly);
    //    builder.AddMediatorPattern(assembly);
    //    builder.AddStrategyPattern();
    //    builder.AddFactoryMethodPattern();
    //    return builder;
    //}

    //private static void AddValidationsBusinessRule(this IHostApplicationBuilder builder, Assembly assembly)
    //{
    //    builder.Services.AddValidatorsFromAssembly(assembly);
    //    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    //    builder.Services.AddSingleton<IDateTimeProvider, BrazilDateTimeProvider>();
    //}

    //private static void AddMediatorPattern(this IHostApplicationBuilder builder, Assembly assembly)
    //{
    //    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
    //}

    //private static void AddStrategyPattern(this IHostApplicationBuilder builder)
    //{
    //    builder.Services.AddScoped<IPaymentStrategy, PixPaymentStrategy>();
    //    builder.Services.AddScoped<IPaymentStrategy, CreditCardPaymentStrategy>();
    //    builder.Services.AddScoped<IPaymentStrategy, PaypalPaymentStrategy>();
    //}

    //private static void AddFactoryMethodPattern(this IHostApplicationBuilder builder)
    //{
    //    builder.Services.AddScoped<IPaymentFactory, PaymentFactory>();
    //}

    //public static IApplicationBuilder UseExceptionHandlingApplication(this IApplicationBuilder app)
    //{
    //    app.UseMiddleware<ExceptionHandlingMiddleware>();
    //    return app;
    //}
}