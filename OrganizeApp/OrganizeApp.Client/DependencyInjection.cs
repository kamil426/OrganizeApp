using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.HttpRepository;
using Radzen;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace OrganizeApp.Client
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddClient(this IServiceCollection services, Uri uri)
        {
            services.AddHttpClient("OrganizeAppAPI", client =>
            {
                client.BaseAddress = uri;
                client.Timeout = TimeSpan.FromMinutes(5);
            });

            services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("OrganizeAppAPI"));
            services.AddScoped<ITaskHttpRepository ,TaskHttpRepository>();
            services.AddRadzenComponents();

            return services;
        }
    }
}
