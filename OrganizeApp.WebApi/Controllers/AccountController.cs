using Microsoft.AspNetCore.Mvc;
using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Shared.Authentication.Dtos;
using OrganizeApp.Shared.Common.Models;

namespace OrganizeApp.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(
            [FromBody] RegisterUserCommand command)
        {
            if (command == null || !ModelState.IsValid)
                return BadRequest();

            await Mediator.Send(command);

            return Ok(new ResponseDto { IsSuccess = true });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginUserCommand command)
        {
            var result = await Mediator.Send(command);

            if (!result.IsAuthSuccessful)
            {
                return Unauthorized(new LoginUserDto
                {
                    ErrorMessage = result.ErrorMessage
                });
            }

            return Ok(result);
        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordCommand command)
        {
            await Mediator.Send(command);

            return Ok(new ResponseDto { IsSuccess = true });
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest("Nieprawidłowe dane.");

            await Mediator.Send(command);

            return Ok(new ResponseDto { IsSuccess = true });
        }

        [HttpGet("EmailConfirmation")]
        public async Task<IActionResult> EmailConfirmation(
            [FromQuery] string email,
            [FromQuery] string token)
        {
            await Mediator.Send(new EmailConfirmationCommand { Email = email, Token = token });

            return Ok();
        }
    }
}
