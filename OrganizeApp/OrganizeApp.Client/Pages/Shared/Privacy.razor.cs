using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using OrganizeApp.Client.Services;

namespace OrganizeApp.Client.Pages.Shared
{
    public partial class Privacy
    {
        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await RefreshLoginStatusService.RefreshLoginHeader("/privacy");
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
