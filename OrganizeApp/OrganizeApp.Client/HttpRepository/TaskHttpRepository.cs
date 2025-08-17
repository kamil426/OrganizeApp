using OrganizeApp.Client.HttpRepository.Interfaces;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using System.Net.Http.Json;

namespace OrganizeApp.Client.HttpRepository
{
    public class TaskHttpRepository : ITaskHttpRepository
    {
        private readonly HttpClient _client;

        public TaskHttpRepository(HttpClient client)
            => _client = client;

        public async Task Add(AddTaskCommand command)
            => await _client.PostAsJsonAsync("task", command);

        public async Task ChangeStatus(ChangeStatusTaskCommand command)
            => await _client.PutAsJsonAsync("task/status", command);

        public async Task DeleteTask(int id, string userId)
            => await _client.DeleteAsync($"task/{id}/{userId}");

        public async Task Edit(EditTaskCommand command)
            => await _client.PutAsJsonAsync("task/task", command);

        public async Task<EditTaskCommand> GetEditTask(int id, string userId)
            => await _client.GetFromJsonAsync<EditTaskCommand>($"task/task-edit/{id}/{userId}");

        public async Task<TaskMoreInfoDto> GetMoreInfo(int id, string userId)
            => await _client.GetFromJsonAsync<TaskMoreInfoDto>($"task/task-info/{id}/{userId}");

        public async Task<IList<TaskAllDto>> GetTasks(string userId)
            => await _client.GetFromJsonAsync<IList<TaskAllDto>>($"task/{userId}");

        public async Task<IList<TasksCheckListDto>> GetTasksCheckList(string userId)
            => await _client.GetFromJsonAsync<IList<TasksCheckListDto>>($"task/check-list/{userId}");
    }
}
