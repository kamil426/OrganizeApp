using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Authentication.Commands;

namespace OrganizeApp.Client.Pages.Authorization
{
    public partial class Login
    {
        private LoginUserCommand _command = new LoginUserCommand();
        public bool _showLoginError;
        public string _error;
        private bool _isLoading = true;

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IAuthenticationHttpRepository AuthenticationRepo { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Save()
        {
            _showLoginError = false;

            var response = await AuthenticationRepo.Login(_command);

            if (!response.IsAuthSuccessful)
            {
                _error = response.ErrorMessage;
                _showLoginError = true;
                return;
            }

            NavigationManager.NavigateTo("/");
        }
    }
}
