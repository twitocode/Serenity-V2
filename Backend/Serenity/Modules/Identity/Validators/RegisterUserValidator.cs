using FluentValidation;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(p => p.Email).EmailAddress().NotEmpty();
        RuleFor(p => p.Username).MinimumLength(4).NotEmpty();
        RuleFor(p => p.Password).MinimumLength(6).NotEmpty();
    }
}