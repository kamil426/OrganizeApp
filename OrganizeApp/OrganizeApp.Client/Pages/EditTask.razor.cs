using Microsoft.AspNetCore.Components;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Commands;

namespace OrganizeApp.Client.Pages
{
    public partial class EditTask
    {
        private EditTaskCommand _task; 

        private bool _isLoading = false;
        private bool _isPrerendering = true;
        private bool _isDateOfPlannedEndDisabled = true;

        [Parameter]
        public int Id { get; set; }

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _task = await TaskHttpRepository.GetEditTask(Id);
                _isPrerendering = false;
                if (_task.DateOfPlannedStart.HasValue)
                    _isDateOfPlannedEndDisabled = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Edit()
        {
            try
            {
                _isLoading = true;
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
    }
}
