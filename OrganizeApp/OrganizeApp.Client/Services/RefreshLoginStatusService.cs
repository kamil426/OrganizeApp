using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using OrganizeApp.Client.Models;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Linq.Dynamic.Core.Tokenizer;

namespace OrganizeApp.Client.Services
{
    public class RefreshLoginStatusService
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly AuthenticationStateProvider _authStateProvider;
        private IJSObjectReference _jsModule;
        private readonly NavigationManager _navigationManager;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly HttpClient _httpClient;

        public RefreshLoginStatusService(IJSRuntime jSRuntime, AuthenticationStateProvider authStateProvider, NavigationManager navigationManager, RefreshTokenService refreshTokenService, HttpClient httpClient)
        {
            _jSRuntime = jSRuntime;
            _authStateProvider = authStateProvider;
            _navigationManager = navigationManager;
            _refreshTokenService = refreshTokenService;
            _httpClient = httpClient;
        }

        public async Task RefreshLoginHeader(string href = null)
        {
            _jsModule = await _jSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
            var token = await _refreshTokenService.TryRefreshToken();

            if (token is null)
            {
                await _jsModule.InvokeVoidAsync("SetLoginHeader", null, _navigationManager.ToAbsoluteUri(href));
                return;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

            var authState = await _authStateProvider.GetAuthenticationStateAsync();

            await _jsModule.InvokeVoidAsync("SetLoginHeader", authState.User.Identity.Name, _navigationManager.ToAbsoluteUri(href));
        }
    }
}
