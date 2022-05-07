using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Common.Errors;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity.Handlers;

public record RegisterUserCommand(RegisterUserDto Dto) : IRequest<Response<object>>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Response<object>>
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public RegisterUserCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<Response<object>> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(command.Dto.Email) is not null)
        {
            return new()
            {
                Errors = new() { new("UserAlreadyExists", "User already exists in the database") },
                Success = false,
                Data = null
            };
        }

        User user = mapper.Map<User>(command.Dto);
        IdentityResult result = await userManager.CreateAsync(user, command.Dto.Password);

        if (result.Succeeded)
        {
            return new()
            {
                Errors = null,
                Success = true,
                Data = null
            };
        }

        List<ApplicationError> errors = new();

        foreach (var error in result.Errors.ToList())
        {
            errors.Add(new ApplicationError(error.Code, error.Description));
        }

        return new()
        {
            Errors = errors,
            Data = null,
            Success = false,
        };
    }
}