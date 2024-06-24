using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Authentication.Commands;
using System.Net;

namespace OrganizeApp.Client.Pages.Authorization
{
    public partial class ForgotPassword
    {
        private ForgotPasswordCommand _command = new ForgotPasswordCommand();
        private bool _showSuccess;
        private bool _showError;
        private bool _isLoading = true;

        [Inject]
        public IAuthenticationHttpRepository AuthenticationRepository { get; set; }

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
            _showSuccess = _showError = false;

            var result = await AuthenticationRepository.ForgotPassword(_command);

            if (result == HttpStatusCode.OK)
                _showSuccess = true;
            else
                _showError = true;
        }
    }
}
