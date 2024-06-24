using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Shared.Authentication.Dtos;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : BaseApiController
    {
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(
            [FromBody] RefreshTokenCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(new LoginUserDto
                {
                    ErrorMessage = "Nieprawidłowe dane."
                });

            var result = await Mediator.Send(command);

            return Ok(result);
        }
    }
}
