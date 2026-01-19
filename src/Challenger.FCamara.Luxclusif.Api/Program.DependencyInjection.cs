using Challenger.FCamara.Luxclusif.Application;
using Challenger.FCamara.Luxclusif.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Challenger.FCamara.Luxclusif.Api;

/// <summary>
///
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        var culture = CultureInfo.CreateSpecificCulture("pt-BR");
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        builder.Services.AddControllers(option =>
        {
            option.RespectBrowserAcceptHeader = true;
            option.ReturnHttpNotAcceptable = true;
            option.AllowEmptyInputInBodyModelBinding = true;
        });

        builder.Services
            .Configure<RouteOptions>(option => { option.LowercaseUrls = true; })
            .Configure<ApiBehaviorOptions>(option => { option.SuppressModelStateInvalidFilter = true; }); //Suprime a validação automática do ModelState para que o FluentValidation seja o único responsável

        builder.Services.AddOpenApi();

        builder.Services.ConfigureHttpJsonOptions(option =>
        {
            option.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            option.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);

        return builder;
    }
}