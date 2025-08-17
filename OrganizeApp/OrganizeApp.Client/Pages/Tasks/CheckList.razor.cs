using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Task.Dtos;
using TaskStatus = OrganizeApp.Shared.Common.Enums.TaskStatus;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class CheckList
    {
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);
        private IList<TasksCheckListDto> _tasksList;
        private IList<TasksCheckListDto> _tasksListToDo;
        private IList<TasksCheckListDto> _tasksListInProgress;
        private IList<TasksCheckListDto> _tasksListComplete;
        private bool _isLoading = true;
        private IJSObjectReference _jsModule;

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public HttpInterceptorService Interceptor { get; set; }

        [Inject]
        public AuthenticationStateProvider AuthStateProvider { get; set; }

        [Inject]
        public RefreshLoginStatusService RefreshLoginStatusService { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [SupplyParameterFromQuery(Name = "ToDo")]
        public bool IsToDoChecked { get; set; }

        [SupplyParameterFromQuery(Name = "InProgress")]
        public bool IsInProgressChecked { get; set; }

        [SupplyParameterFromQuery(Name = "Complete")]
        public bool IsCompleteChecked { get; set; }

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
                var authState = await AuthStateProvider.GetAuthenticationStateAsync();
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
                _tasksList = await TaskHttpRepository.GetTasksCheckList(authState.GetUserId());
                PrepareTasksLists();
                _isLoading = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
        }

        private void PrepareTasksLists()
        {
            _tasksListInProgress = _tasksList.Where(x => x.TaskStatus == TaskStatus.InProgress).ToList();
            _tasksListToDo = _tasksList.Where(x => x.TaskStatus == TaskStatus.ToDo).ToList();
            _tasksListComplete = _tasksList.Where(x => x.TaskStatus == TaskStatus.Complete).ToList();
        }

        private async Task Print(MouseEventArgs args)
        {
            await _jsModule.InvokeVoidAsync("Print");
        }

        private void Return(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("/tasks");
        }
    }
}
