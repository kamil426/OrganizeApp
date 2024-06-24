using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.Client.HttpRepository.Interfaces
{
    public interface ITaskHttpRepository
    {
        Task Add(AddTaskCommand command);
        Task<IList<TaskAllDto>> GetTasks(string userId);
        Task ChangeStatus(ChangeStatusTaskCommand command);
        Task<EditTaskCommand> GetEditTask(int id, string userId);
        Task Edit(EditTaskCommand command);
        Task<TaskMoreInfoDto> GetMoreInfo(int id, string userId);
        Task DeleteTask(int id, string userId);
    }
}
