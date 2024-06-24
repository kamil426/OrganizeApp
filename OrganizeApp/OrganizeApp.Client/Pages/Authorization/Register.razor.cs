using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Authentication.Commands;

namespace OrganizeApp.Client.Pages.Authorization
{
    public partial class Register
    {
        private RegisterUserCommand _command = new RegisterUserCommand();
        private bool _showErrors;
        private IEnumerable<string> _errors;
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
            _showErrors = false;

            var response = await AuthenticationRepo.RegisterUser(_command);

            if (!response.IsSuccess)
            {
                _errors = new List<string> { response.Errors };
                _showErrors = true;
                return;
            }

            NavigationManager.NavigateTo("/confirm-email");
        }
    }
}
