using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Common.Enums;
using OrganizeApp.Shared.Task.Commands;

namespace OrganizeApp.Client.Pages
{
    public partial class AddTask
    {
        private AddTaskCommand _task = new AddTaskCommand();

        private bool _isLoading = false;
        private bool _isDateOfPlannedEndDisabled = true;

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        private async Task Add()
        {
            try
            {
                _isLoading = true;
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
    }
}
