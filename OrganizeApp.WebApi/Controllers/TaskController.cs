using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            return Ok(await Mediator.Send(new GetTasksQuery()));
        }

        [HttpGet("task-edit/{id}")]
        public async Task<IActionResult> GetToEditTask(int id)
        {
            var task = await Mediator.Send(new GetToEditTaskQuery { Id = id});

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("task-info/{id}")]
        public async Task<IActionResult> GetMoreInfoTask(int id)
        {
            var task = await Mediator.Send(new GetMoreInfoTaskQuery { Id = id });

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddTaskCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPut("status")]
        public async Task<IActionResult> ChangeStatus(ChangeStatusTaskCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("task")]
        public async Task<IActionResult> Edit(EditTaskCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTaskCommand() { Id = id });
            return NoContent();
        }
    }
}
