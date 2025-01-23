using MudBlazor.Services;
using MudBlazor.Translations;
using Overseer.WebUI;
using Overseer.WebUI.Components;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRedisOutputCache("overseer-redis-cache");
builder.Services.AddMudServices();
builder.Services.AddMudTranslations();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

const string webApiAddress = "https+http://overseer-web-api";
builder.Services.AddHttpClient<WeatherApiClient>(client =>
    client.BaseAddress = new Uri(webApiAddress));

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

await app.RunAsync();
