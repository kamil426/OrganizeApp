using Microsoft.AspNetCore.Components;
using OrganizeApp.Client.HttpRepository;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Dtos;

namespace OrganizeApp.Client.Pages
{
    public partial class TaskRead
    {
        private TaskMoreInfoDto _task;
        private bool _isPrerendering = true;

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
                _task = await TaskHttpRepository.GetMoreInfo(Id);
                _isPrerendering = false;
                StateHasChanged();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private void ReturnToMyTasks()
        {
            NavigationManager.NavigateTo("/tasks");
        }
    }
}
