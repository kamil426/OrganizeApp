using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Dtos;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.Client.HttpRepository.Interfaces
{
    public interface ITaskHttpRepository
    {
        Task Add(AddTaskCommand command);
        Task<IList<TaskDto>> GetTasks(string userId);
        Task ChangeStatus(ChangeStatusTaskCommand command);
        Task<EditTaskCommand> GetToEditTask(int id, string userId);
        Task Edit(EditTaskCommand command);
        Task<TaskMoreInfoDto> GetMoreInfo(int id, string userId);
        Task Delete(int id, string userId);
        Task<IList<TaskCheckListDto>> GetTasksCheckList(string userId);
        Task<IList<TaskIncludingCategoryDto>> GetTasksIncludingCategory(string userId);
    }
}
