using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.Services;

namespace OrganizeApp.Client.Pages.Shared
{
    public partial class Contact
    {
        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await RefreshLoginStatusService.RefreshLoginHeader("/contact");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
