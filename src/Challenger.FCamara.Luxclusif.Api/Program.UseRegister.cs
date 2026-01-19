using Challenger.FCamara.Luxclusif.Application;
using Scalar.AspNetCore;

namespace Challenger.FCamara.Luxclusif.Api;

/// <summary>
///
/// </summary>
public static class UseRegister
{
    /// <summary>
    /// Configura a pipeline de requisições HTTP.
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication UseServices(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference("/docs");
        }

        app.UseExceptionHandlingApplication();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}