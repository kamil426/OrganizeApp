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

        public async Task DeleteTask(int id)
            => await _client.DeleteAsync($"task/{id}");

        public async Task Edit(EditTaskCommand command)
            => await _client.PutAsJsonAsync("task/task", command);

        public async Task<EditTaskCommand> GetEditTask(int id)
            => await _client.GetFromJsonAsync<EditTaskCommand>($"task/task-edit/{id}");

        public async Task<TaskMoreInfoDto> GetMoreInfo(int id)
            => await _client.GetFromJsonAsync<TaskMoreInfoDto>($"task/task-info/{id}");

        public async Task<IList<TaskAllDto>> GetTasks()
            => await _client.GetFromJsonAsync<IList<TaskAllDto>>("task");
    }
}
