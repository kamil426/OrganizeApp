using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OrganizeApp.Client.HttpRepository;
using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Dtos;
using Radzen;
using Radzen.Blazor;
using Radzen.Blazor.Rendering;
using System.Drawing;

namespace OrganizeApp.Client.Pages
{
    public partial class Scheduler
    {
        private RadzenScheduler<TaskAllDto> _scheduler;
        private IList<TaskAllDto> _tasks;
        private bool _isLoading = true;
        private DateTime _date;
        private IJSObjectReference _jsModule;
        private int _selectedIndex = 2;

        [Parameter]
        public string? Data { get; set; }

        [Parameter]
        public string? View { get; set; }

        [Inject]
        public ITaskHttpRepository TaskHttpRepository { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            _date = Data is not null ? DateTime.Parse(Data) : DateTime.Now;

            if(View is not null)
            {
                switch (View)
                {
                    case "Dzień":
                        _selectedIndex = 0;
                        break;
                    case "Tydzień":
                        _selectedIndex = 1;
                        break;
                    case "Miesiąc":
                        _selectedIndex = 2;
                        break;
                    default:
                        break;
                }
            }

            await base.OnParametersSetAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./scripts/JavaScript.js");
                await Refresh();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        private async Task Refresh()
        {
            _tasks = await TaskHttpRepository.GetTasks();
            PrepareTasks();
            _isLoading = false;
            StateHasChanged();
        }

        private void OpenDescription(SchedulerAppointmentSelectEventArgs<TaskAllDto> e)
        {
            NavigationManager.NavigateTo($"/task/read/{e.Data.Id}/task*scheduler*{_scheduler.CurrentDate}*{_scheduler.SelectedView.Text}");
        }

        private void OnAppointmentRender(SchedulerAppointmentRenderEventArgs<TaskAllDto> e)
        {
            if (e.Data.TaskStatus == Shared.Common.Enums.TaskStatus.Complete)
            {
                e.Attributes["style"] = "background: #299c1a;";
            }
            else if (e.Data.TaskStatus == Shared.Common.Enums.TaskStatus.InProgress)
            {
                if(DateTime.Now > e.Data.DateOfPlannedEnd.Value)
                {
                    e.Attributes["style"] = "background-color: #ff0000;";
                }
                else if(e.Data.DateOfPlannedStart.Value < DateTime.Now && DateTime.Now < e.Data.DateOfPlannedEnd.Value)
                {
                    e.Attributes["style"] = "background-color: #bebe37;";
                }
            }
        }

        private void OnSlotRender(SchedulerSlotRenderEventArgs e)
        {
            if (e.View.Text == "Miesiąc" && e.Start.Date.DayOfWeek == DayOfWeek.Saturday)
            {
                e.Attributes["style"] = "background: rgb(154 154 154 / 20%);";
            }

            if (e.View.Text == "Miesiąc" && e.Start.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                e.Attributes["style"] = "background: rgb(239 80 80 / 20%);";
            }

            if (e.View.Text == "Miesiąc" && e.Start.Date == DateTime.Today)
            {
                e.Attributes["style"] = "box-shadow: inset 0px 0px 0px 2px rgba(67, 64, 210, 1);";
                if (e.View.Text == "Miesiąc" && e.Start.Date.DayOfWeek == DayOfWeek.Saturday)
                {
                    e.Attributes["style"] = "background: rgb(154 154 154 / 20%);" +
                        "box-shadow: inset 0px 0px 0px 2px rgba(67, 64, 210, 1);";
                }

                if (e.View.Text == "Miesiąc" && e.Start.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    e.Attributes["style"] = "background: rgb(239 80 80 / 20%);" +
                        "box-shadow: inset 0px 0px 0px 2px rgba(67, 64, 210, 1);";
                }
            }

            if (e.View.Text == "Tydzień" && e.Start.DayOfWeek == DayOfWeek.Saturday)
            {
                e.Attributes["style"] = "background: rgb(154 154 154 / 20%);";
            }

            if (e.View.Text == "Tydzień" && e.Start.DayOfWeek == DayOfWeek.Sunday)
            {
                e.Attributes["style"] = "background: rgb(239 80 80 / 20%);";
            }
        }

        private async void OnClick()
        {
            await _jsModule.InvokeVoidAsync("SetSchedulerHeight", _scheduler.SelectedView.Text);
        }

        private void PrepareTasks()
        {
            foreach (var task in _tasks)
            {
                if (task.DateOfPlannedStart.HasValue)
                {
                    if (task.DateOfComplete.HasValue)
                    {
                        task.DateOfPlannedEnd = new DateTime?(task.DateOfComplete.Value);
                        continue;
                    }
                    if (!task.DateOfPlannedEnd.HasValue)
                    {
                        task.DateOfPlannedEnd = new DateTime?(task.DateOfPlannedStart.Value.AddHours(1));
                    }
                }
            }
        }
    }
}
