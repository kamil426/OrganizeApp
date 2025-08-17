using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using OrganizeApp.Client.Components;
using OrganizeApp.Client.Extensions;
using OrganizeApp.Client.HttpInterceptor;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Client.Services;
using OrganizeApp.Shared.Common.Models;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using System.Security.Claims;
using TaskStatus = OrganizeApp.Shared.Common.Enums.TaskStatus;

namespace OrganizeApp.Client.Pages.Tasks
{
    public partial class Tasks : IDisposable //ToDo: zaszyfrowanie hasła do emaila, kategorie, zmniejszenie rozmiarów tasków, sortowanie tasków
    {
        private IList<TaskAllDto> _tasksList;
        private ChangeStatusTaskCommand _task;
        private List<int> _deletingTasksId = new();
        private Modal _modalDeleteTasks;
        private Modal _modalCheckListConfiguration;
        private IJSObjectReference _jsModule;
        private bool _isLoading = true;
        private bool _isDeleteTrybeEnabled = false;
        private DotNetObjectReference<Tasks>? _dotNetHelper;
        private static IComponentRenderMode _renderMode = new InteractiveAutoRenderMode(prerender: false);
        private IEnumerable<CheckListConfiguration> _checkListConfiguration;
        private bool _isCheckListConfigValid = true;

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

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
            _checkListConfiguration = new List<CheckListConfiguration>
            {
                new() {StatusName = TaskStatus.ToDo.ToString(), IsChecked = false },
                new() {StatusName = TaskStatus.InProgress.ToString(), IsChecked = false },
                new() {StatusName = TaskStatus.Complete.ToString(), IsChecked = false },
            };
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await RefreshLoginStatusService.RefreshLoginHeader("/tasks");
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
                _dotNetHelper = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("Helpers.setDotNetHelper", _dotNetHelper);
                await RefreshTasks();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RefreshTasks()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity.IsAuthenticated is false)
                NavigationManager.NavigateTo("/login");
            _tasksList = await TaskHttpRepository.GetTasks(authState.GetUserId());
            _isLoading = false;
            StateHasChanged();
            await _jsModule.InvokeVoidAsync("CreateDroppable");
        }

        [JSInvokable("OnDrop")]
        public async Task OnDrop(int taskId, int statusId)
        {
            var dragModel = _tasksList.SingleOrDefault(x => x.Id == taskId);
            var status = (TaskStatus)statusId;

            if (dragModel is null)
                return;

            if (dragModel.TaskStatus == status)
                return;

            dragModel.TaskStatus = status;

            if (dragModel.TaskStatus == TaskStatus.Complete)
                dragModel.DateOfComplete = DateTime.Now;
            else if (dragModel.DateOfComplete is not null)
                dragModel.DateOfComplete = null;

            var authState = await AuthStateProvider.GetAuthenticationStateAsync();

            _task = new ChangeStatusTaskCommand()
            {
                Id = dragModel.Id,
                DateOfComplete = dragModel.DateOfComplete,
                TaskStatus = dragModel.TaskStatus,
                UserId = authState.GetUserId(),
            };

            await TaskHttpRepository.ChangeStatus(_task);
            await RefreshTasks();
        }

        private async void OnMouseEnter(int taskId)
        {
            await _jsModule.InvokeVoidAsync("CreateDraggable", taskId);
        }


        private void AddTaskToListDeletingItems(int id, ChangeEventArgs e)
        {
            if (Convert.ToBoolean(e.Value))
            {
                _deletingTasksId.Add(id);
                return;
            }

            var uncheckTask = _deletingTasksId.SingleOrDefault(x => x == id);
            if (uncheckTask is 0)
                return;

            _deletingTasksId.Remove(uncheckTask);
        }

        private void Delete()
        {
            if (_isDeleteTrybeEnabled && _deletingTasksId.Count > 0)
            {
                _modalDeleteTasks.Open();
                return;
            }
            _isDeleteTrybeEnabled = true;
        }

        private async void DeleteTasks(AuthenticationState authState)
        {
            if (_deletingTasksId.Count > 0)
                foreach (var taskId in _deletingTasksId)
                {
                    await TaskHttpRepository.DeleteTask(taskId, authState.GetUserId());
                    var taskToDelete = _tasksList.SingleOrDefault(x => x.Id == taskId);
                    if (taskToDelete is not null)
                        _tasksList.Remove(taskToDelete);
                }
            _modalDeleteTasks.Close();
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        private void CancelDelete()
        {
            _modalDeleteTasks.Close();
        }

        private void GenerateCheckList()
        {
            _modalCheckListConfiguration.Open();
        }

        private async void CancelDeleteTrybe()
        {
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
            _deletingTasksId.Clear();
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        private void EditTask(int id)
        {
            NavigationManager.NavigateTo($"/task/edit/{id}");
        }

        private void OpenDescription(int id)
        {
            NavigationManager.NavigateTo($"/task/read/{id}/tasks");
        }

        public void Dispose()
        {
            Interceptor.DisposeEvent();
            _dotNetHelper?.Dispose();
        }

        private void ConfigCheckList()
        {
            if(_checkListConfiguration.FirstOrDefault(x => x.IsChecked == true) == null)
            {
                _isCheckListConfigValid = false;
                return;
            }
            NavigationManager.NavigateTo($"/tasks/checklist?" +
                $"ToDo={_checkListConfiguration.ElementAt(Convert.ToInt32(TaskStatus.ToDo) - 1).IsChecked}&" +
                $"InProgress={_checkListConfiguration.ElementAt(Convert.ToInt32(TaskStatus.InProgress) - 1).IsChecked}&" +
                $"Complete={_checkListConfiguration.ElementAt(Convert.ToInt32(TaskStatus.Complete) - 1).IsChecked}");
        }

        private void ConfigCheckListChanged(TaskStatus status, ChangeEventArgs e)
        {
            if(!_isCheckListConfigValid && Convert.ToBoolean(e.Value) == true)
                _isCheckListConfigValid = true;
            _checkListConfiguration.ElementAt(Convert.ToInt32(status) - 1).IsChecked = Convert.ToBoolean(e.Value);
        }
    }
}
