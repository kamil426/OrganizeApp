using Microsoft.AspNetCore.Components;
using OrganizeApp.Client.Services;
using System.Net;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace OrganizeApp.Client.HttpInterceptor
{
    public class HttpInterceptorService
    {
        private readonly HttpClientInterceptor _interceptor;
        private readonly NavigationManager _navigationManager;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly NavigationManager _navManager;

        public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navigationManager, RefreshTokenService refreshTokenService, NavigationManager navManager)
        {
            _interceptor = interceptor;
            _navigationManager = navigationManager;
            _refreshTokenService = refreshTokenService;
            _navManager = navManager;
        }

        public void RegisterAfterSendAsyncEvent() =>
            _interceptor.AfterSendAsync += HandleResponse;

        public void RegisterBeforeSendAsyncEvent() =>
            _interceptor.BeforeSendAsync += InterceptBeforeSendAsync;

        public void DisposeEvent()
        {
            _interceptor.AfterSendAsync -= HandleResponse;
            _interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
        }

        private async Task InterceptBeforeSendAsync(object sender,
    HttpClientInterceptorEventArgs e)
        {
            var absolutePath = e.Request.RequestUri.AbsolutePath;

            if (!absolutePath.Contains("token") && !absolutePath.Contains("account"))
            {
                var token = await _refreshTokenService.TryRefreshToken();

                if (!string.IsNullOrEmpty(token))
                {
                    e.Request.Headers.Authorization =
                        new AuthenticationHeaderValue("bearer", token);
                }
            }
        }

        private async Task HandleResponse(object sender, HttpClientInterceptorEventArgs e)
        {
            if (e.Response is null)
            {
                _navigationManager.NavigateTo("/error");
                return;
            }

            var message = string.Empty;

            if (!e.Response.IsSuccessStatusCode)
            {
                switch (e.Response.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        _navigationManager.NavigateTo("/404");
                        message = "Nie znaleziono zasobu";
                        break;
                    case HttpStatusCode.Unauthorized:
                        _navManager.NavigateTo("/login");
                        message = "Dostęp zabroniony";
                        break;
                    default:
                        _navigationManager.NavigateTo("/error");
                        message = "Coś poszło nie tak, proszę skontaktuj się z administratorem";
                        break;
                }
                throw new HttpResponseException(message);
            }
        }
    }
}
