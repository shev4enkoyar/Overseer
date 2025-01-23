using Microsoft.OpenApi.Models;
using Overseer.WebAPI.Application;
using Overseer.WebAPI.Infrastructure;
using Overseer.WebAPI.Infrastructure.Data;
using Scalar.AspNetCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddApplicationServices()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IApiErrorHandler, ApiErrorHandler>();

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi(options => options.AddDocumentTransformer((document, _, _) =>
{
    document.Info = new OpenApiInfo
    {
        Title = "Overseer",
        Version = "v1",
        Description = "A simple web API for managing projects."
    };
    document.Servers.Add(new OpenApiServer { Url = "http://localhost:5448" });
    return Task.CompletedTask;
}));

WebApplication app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    await app.InitializeDatabaseAsync();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
        options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient)
            .WithTitle("Overseer")
            .WithTheme(ScalarTheme.DeepSpace));
}

string[] summaries =
    ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

app.MapGet("/weatherforecast", () =>
    {
        WeatherForecast[] forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
#pragma warning disable CA5394
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
#pragma warning restore CA5394
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapDefaultEndpoints();

app.MapEndpoints();

await app.RunAsync();

#pragma warning disable S3903
internal sealed record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
#pragma warning restore S3903
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
