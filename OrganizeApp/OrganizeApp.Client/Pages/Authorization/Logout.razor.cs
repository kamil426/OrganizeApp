using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.HttpRepository.Interfaces;

namespace OrganizeApp.Client.Pages.Authorization
{
    public partial class Logout
    {
        static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Inject]
        public IAuthenticationHttpRepository AuthenticationHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await AuthenticationHttpRepository.Logout();
            NavigationManager.NavigateTo("/");
        }
    }
}
