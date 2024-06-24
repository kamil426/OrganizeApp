using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TaskController : BaseApiController
    {
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetTasks(string userId)
        {
            return Ok(await Mediator.Send(new GetTasksQuery { UserId = userId}));
        }

        [HttpGet("task-edit/{id}/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetToEditTask(int id, string userId)
        {
            var task = await Mediator.Send(new GetToEditTaskQuery { Id = id, UserId = userId});

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpGet("task-info/{id}/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetMoreInfoTask(int id, string userId)
        {
            var task = await Mediator.Send(new GetMoreInfoTaskQuery { Id = id, UserId = userId });

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddTaskCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        [HttpPut("status")]
        [Authorize]
        public async Task<IActionResult> ChangeStatus(ChangeStatusTaskCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpPut("task")]
        [Authorize]
        public async Task<IActionResult> Edit(EditTaskCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, string userId)
        {
            await Mediator.Send(new DeleteTaskCommand() { Id = id, UserId = userId });
            return NoContent();
        }
    }
}
