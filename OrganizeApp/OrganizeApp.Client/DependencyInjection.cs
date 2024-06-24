using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.HttpRepository;
using Radzen;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.AuthStateProviders;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using OrganizeApp.Client.HttpInterceptor;
using Blazored.LocalStorage;
using OrganizeApp.Client.Services;

namespace OrganizeApp.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClient(this IServiceCollection services, Uri uri)
        {
            services.AddHttpClient("OrganizeAppAPI", (sp, client) =>
            {
                client.BaseAddress = uri;
                client.Timeout = TimeSpan.FromMinutes(5);
                client.EnableIntercept(sp);
            });

            services.AddHttpClientInterceptor();
            services.AddScoped<HttpInterceptorService>();

            services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("OrganizeAppAPI"));
            services.AddScoped<ITaskHttpRepository ,TaskHttpRepository>();
            services.AddRadzenComponents();

            services.AddBlazoredLocalStorage();

            services.AddOptions();
            services.AddAuthorizationCore();

            services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
            services.AddCascadingAuthenticationState();

            services.AddScoped<IAuthenticationHttpRepository, AuthentiactionHttpRepository>();
            services.AddScoped<RefreshTokenService>();
            services.AddScoped<RefreshLoginStatusService>();

            return services;
        }
    }
}
