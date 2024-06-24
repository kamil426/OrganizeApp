using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.WebUtilities;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Shared.Authentication.Dtos;
using OrganizeApp.Shared.Common.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace OrganizeApp.Client.HttpRepository
{
    public class AuthentiactionHttpRepository : IAuthenticationHttpRepository
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly JsonSerializerOptions _options =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public AuthentiactionHttpRepository(HttpClient client, ILocalStorageService localStorage, NavigationManager navigationManager, AuthenticationStateProvider authStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _authStateProvider = authStateProvider;
        }

        public async Task<HttpStatusCode> EmailConfirmation(string email, string token)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["email"] = email,
                ["token"] = token
            };

            var response = await _client.GetAsync(QueryHelpers.AddQueryString(
                "account/emailconfirmation", queryStringParam));

            return response.StatusCode;
        }

        public async Task<HttpStatusCode> ForgotPassword(ForgotPasswordCommand command)
        {
            command.ClientURI =
                Path.Combine(_navigationManager.BaseUri, "reset-password");

            var result = await _client.PostAsJsonAsync("account/forgotpassword",
                command);

            return result.StatusCode;
        }

        public async Task<LoginUserDto> Login(LoginUserCommand userForAuthentication)
        {
            var response = await _client.PostAsJsonAsync("account/login",
                userForAuthentication);

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<LoginUserDto>(content, _options);

            if (!response.IsSuccessStatusCode)
                return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(
                result.Token);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", result.Token);

            return new LoginUserDto { IsAuthSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");


            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

            _client.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var response = await _client.PostAsJsonAsync("token/refresh",
                new RefreshTokenCommand
                {
                    Token = token,
                    RefreshToken = refreshToken
                });

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<LoginUserDto>(content, _options);

            if (!result.IsAuthSuccessful)
                return null;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("bearer", result.Token);

            return result.Token;
        }

        public async Task<ResponseDto> RegisterUser(RegisterUserCommand registerUserCommand)
        {
            registerUserCommand.ClientURI = Path.Combine(
                _navigationManager.BaseUri, "confirmation-email");

            var response = await _client.PostAsJsonAsync("account/register",
                registerUserCommand);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ResponseDto>(content, _options);

                return result;
            }

            return new ResponseDto { IsSuccess = true };
        }

        public async Task<ResponseDto> ResetPassword(ResetPasswordCommand command)
        {
            var resetResult = await _client.PostAsJsonAsync("account/resetpassword",
                command);

            var resetContent = await resetResult.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseDto>(resetContent,
                _options);

            return result;
        }
    }
}
