using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.AuthStateProviders;
using OrganizeApp.Client.Components;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Category.Dtos;
using OrganizeApp.Shared.Common.Enums;
using OrganizeApp.Shared.Task.Commands;
using System.Security.Claims;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class AddTask : IDisposable
    {
        private AddTaskCommand _task = new AddTaskCommand();

        private bool _isLoading = false;
        private bool _isDateOfPlannedEndDisabled = true;
        private bool _isPageLoading = true;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);
        private IList<CategoryDto> _categories;

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public ICategoryHttpRepository CategoryHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                if (authState.User.Identity.IsAuthenticated is false)
                {
                    NavigationManager.NavigateTo("/login");
                    return;
                }
                _categories = await CategoryHttpRepository.GetCategories(authState.GetUserId());
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
                if (_task.CategoryId == 0)
                    _task.CategoryId = null;
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
