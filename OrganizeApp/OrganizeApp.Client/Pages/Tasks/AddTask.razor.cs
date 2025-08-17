using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Common.Enums;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Client.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.Services;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class AddTask : IDisposable //ToDo: addTask event onblur
    {
        private AddTaskCommand _task = new AddTaskCommand();

        private bool _isLoading = false;
        private bool _isDateOfPlannedEndDisabled = true;
        private bool _isPageLoading = true;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshLoginStatusService.RefreshLoginHeader("/task/add");
                _isPageLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected override async Task OnInitializedAsync()
        {
            Interceptor.RegisterBeforeSendAsyncEvent();
            Interceptor.RegisterAfterSendAsyncEvent();
            await base.OnInitializedAsync();
        }

        private async Task Add(AuthenticationState authContext)
        {
            try
            {
                _isLoading = true;
                _task.UserId = authContext.GetUserId();
                await TaskHttpRepository.Add(_task);
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

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }
    }
}
