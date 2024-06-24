using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrganizeApp.Application.Common.Interfaces;
using OrganizeApp.Shared.Authentication.Commands;
using OrganizeApp.Application.Common.Exceptions;
using OrganizeApp.Domain.Entities;

namespace OrganizeApp.Application.Authentication.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async System.Threading.Tasks.Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                throw new ValidationException("Nieprawidłowe dane.");

            var resetPassResult = await _userManager.ResetPasswordAsync(user,
                request.Token, request.Password);

            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => new ValidationFailure { PropertyName = e.Code, ErrorMessage = e.Description });
                throw new ValidationException(errors);
            }

            await _userManager.SetLockoutEndDateAsync(user, null);
        }
    }

}
