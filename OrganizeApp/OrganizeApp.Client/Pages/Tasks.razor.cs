﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrganizeApp.Client.Components;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using TaskStatus = OrganizeApp.Shared.Common.Enums.TaskStatus;

namespace OrganizeApp.Client.Pages
{
    public partial class Tasks : IDisposable//ToDo: Dodać logowanie
    {
        private IList<TaskAllDto> _tasksList;
        private ChangeStatusTaskCommand _task;
        private List<int> _deletingTasksId = new();
        private Modal Modal;
        private IJSObjectReference _jsModule;
        private bool _isLoading = true;
        private bool _isDeleteTrybeEnabled = false;
        private DotNetObjectReference<Tasks>? _dotNetHelper;

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
                _dotNetHelper = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("Helpers.setDotNetHelper", _dotNetHelper);
                await RefreshTasks();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task RefreshTasks()
        {
            _tasksList = await TaskHttpRepository.GetTasks();
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

            _task = new ChangeStatusTaskCommand()
            {
                Id = dragModel.Id,
                DateOfComplete = dragModel.DateOfComplete,
                TaskStatus = dragModel.TaskStatus
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
                Modal.Open();
                return;
            }
            _isDeleteTrybeEnabled = true;
        }

        private async void DeleteTasks()
        {
            if (_deletingTasksId.Count > 0)
                foreach (var taskId in _deletingTasksId)
                {
                    await TaskHttpRepository.DeleteTask(taskId);
                    var taskToDelete = _tasksList.SingleOrDefault(x => x.Id == taskId);
                    if (taskToDelete is not null)
                        _tasksList.Remove(taskToDelete);
                }
            Modal.Close();
            await _jsModule.InvokeVoidAsync("UncheckCheckboxes");
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
            _dotNetHelper?.Dispose();
        }
    }
}
