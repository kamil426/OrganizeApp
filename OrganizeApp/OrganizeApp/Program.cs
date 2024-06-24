using Microsoft.Extensions.DependencyInjection.Extensions;
using OrganizeApp.Client;
using OrganizeApp.Client.HttpRepository;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var uri = new Uri(builder.Configuration["ApiConfiguration:BaseAddress"] + "api/");

builder.Services.AddClient(uri);
builder.Services.AddRadzenComponents();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(_Imports).Assembly);

app.Run();