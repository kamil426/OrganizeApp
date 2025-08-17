using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Task.Commands;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class EditTask : IDisposable //ToDo: addTask event onblur
    {
        private EditTaskCommand _task; 

        private bool _isLoading = false;
        private bool _isDataLoading = true;
        private bool _isDateOfPlannedEndDisabled = true;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Parameter]
        public int Id { get; set; }

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
                _task = await TaskHttpRepository.GetEditTask(Id, authState.GetUserId());
                _isDataLoading = false;
                if (_task.DateOfPlannedStart.HasValue)
                    _isDateOfPlannedEndDisabled = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Edit(AuthenticationState authState)
        {
            try
            {
                _isLoading = true;
                _task.UserId = authState.GetUserId();
                await TaskHttpRepository.Edit(_task);
                NavigationManager.NavigateTo("/tasks");
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void DateOfPlannedStartSelect()
        {
            if (_task.DateOfPlannedStart.HasValue)
                _isDateOfPlannedEndDisabled = false;
            else
            {
                _isDateOfPlannedEndDisabled = true;
                _task.DateOfPlannedEnd = null;
            }
        }

        private void ReturnToMyTasks()
        {
            NavigationManager.NavigateTo("/tasks");
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
    }
}
