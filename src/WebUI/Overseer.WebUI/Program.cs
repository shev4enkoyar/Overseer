using MudBlazor.Services;
using MudBlazor.Translations;
using MudExtensions.Services;
using Overseer.WebUI;
using Overseer.WebUI.Components;
using Overseer.WebUI.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();
builder.Services.AddMudTranslations();
builder.Services.AddMudExtensions();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddTransient<NavigationService>();

const string webApiAddress = "http://overseer.webapi:8080/api/";

builder.Services.AddHttpClient<WeatherApiClient>(static client =>
    client.BaseAddress = new Uri(webApiAddress));

builder.Services.AddHttpClient<OverseerApiClient>(static client =>
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

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync();
