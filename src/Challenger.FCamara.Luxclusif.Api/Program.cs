using Challenger.FCamara.Luxclusif.Api;

await WebApplication.CreateBuilder(args)
    .RegisterServices().Build()
    .UseServices().RunAsync();