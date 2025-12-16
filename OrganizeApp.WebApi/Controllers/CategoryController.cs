using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Shared.Category.Commands;
using OrganizeApp.Shared.Category.Queries;
using OrganizeApp.Shared.Task.Commands;
using OrganizeApp.Shared.Task.Queries;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : BaseApiController
    {
        [HttpGet("categories/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetCategories(string userId)
        {
            return Ok(await Mediator.Send(new GetCategoriesQuery { UserId = userId }));
        }


        [HttpGet("category-edit/{id}/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetToEditCategory(int id, string userId)
        {
            var task = await Mediator.Send(new GetToEditCategoryQuery { Id = id, UserId = userId });

            if (task == null)
                return NotFound();

            return Ok(task);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(AddCategoryCommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }


        [HttpPut("category")]
        [Authorize]
        public async Task<IActionResult> Edit(EditCategoryCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}/{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id, string userId)
        {
            await Mediator.Send(new DeleteCategoryCommand() { Id = id, UserId = userId });
            return NoContent();
        }
    }
}
