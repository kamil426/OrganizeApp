using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;
using OrganizeApp.Client.Components;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using System.Runtime.InteropServices;
using TaskStatus = OrganizeApp.Shared.Common.Enums.TaskStatus;

namespace OrganizeApp.Client.Pages
{
    public partial class Tasks //Todo: dodać animacje drag and drop-a
    {
        private IList<TaskAllDto> _tasksList;
        private ChangeStatusTaskCommand _task;
        private bool _isPrerendering = true;
        private bool _isDeleteTrybeEnabled = false;
        private List<int> _deletingTasksId = new();
        private Modal Modal;
        private IJSObjectReference _jsModule;

        public TaskAllDto DragModel { get; set; }

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
                await RefreshTasks();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RefreshTasks()
        {
            _tasksList = await TaskHttpRepository.GetTasks();
            _isPrerendering = false;
            StateHasChanged();
        }

        private async Task OnDrop(TaskStatus status)
        {
            if (DragModel is null)
                return;
            DragModel.TaskStatus = status;

            if (DragModel.TaskStatus == TaskStatus.Complete)
                DragModel.DateOfComplete = DateTime.Now;
            else if (DragModel.DateOfComplete is not null)
                DragModel.DateOfComplete = null;

            _task = new ChangeStatusTaskCommand()
            {
                Id = DragModel.Id,
                DateOfComplete = DragModel.DateOfComplete,
                TaskStatus = DragModel.TaskStatus
            };

            await TaskHttpRepository.ChangeStatus(_task);
            await RefreshTasks();
        }

        private void OnDragStart(TaskAllDto task, DragEventArgs e)
        {
            DragModel = task;
        }

        private void OnDragEnd(DragEventArgs e)
        {
            DragModel = null;
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
                Modal.Open();
                return;
            }
            _isDeleteTrybeEnabled = true;
        }

        private async void DeleteTasks()
        {
            if(_deletingTasksId.Count > 0)
                foreach (var taskId in _deletingTasksId)
                {
                    await TaskHttpRepository.DeleteTask(taskId);
                    var taskToDelete = _tasksList.SingleOrDefault(x => x.Id == taskId);
                    if(taskToDelete is not null)
                        _tasksList.Remove(taskToDelete);
                }
            Modal.Close();
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        private void CancelDelete()
        {
            Modal.Close();
        }

        private async void CancelDeleteTrybe()
        {
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
            _isDeleteTrybeEnabled = false;
            StateHasChanged();
        }

        private void EditTask(int id)
        {
            NavigationManager.NavigateTo($"/task/edit/{id}");
        }

        private void OpenDescription(int id)
        {
            NavigationManager.NavigateTo($"/task/read/{id}");
        }
    }
}
