using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using OrganizeApp.Client;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.HttpRepository;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var uri = new Uri(builder.Configuration["ApiConfiguration:BaseAddress"] + "api/");

builder.Services.AddClient(uri);

await builder.Build().RunAsync();
