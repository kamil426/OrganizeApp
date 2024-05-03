using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.Client.HttpRepository.Interfaces
{
    public interface ITaskHttpRepository
    {
        Task Add(AddTaskCommand command);
        Task<IList<TaskAllDto>> GetTasks();
        Task ChangeStatus(ChangeStatusTaskCommand command);
        Task<EditTaskCommand> GetEditTask(int id);
        Task Edit(EditTaskCommand command);
        Task<TaskMoreInfoDto> GetMoreInfo(int id);
        Task DeleteTask(int id);
    }
}
