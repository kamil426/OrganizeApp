using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Task.Dtos;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class TaskRead : IDisposable
    {
        private TaskMoreInfoDto _task;
        private bool _isLoading = true;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Parameter]
        public int Id { get; set; }

        [Parameter]
        public string Url { get; set; }

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterBeforeSendAsyncEvent();
            Interceptor.RegisterAfterSendAsyncEvent();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshLoginStatusService.RefreshLoginHeader();
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                _task = await TaskHttpRepository.GetMoreInfo(Id, authState.GetUserId());
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void Return()
        {
            NavigationManager.NavigateTo($"/{Url.Replace("*", "/")}");
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
    }
}
