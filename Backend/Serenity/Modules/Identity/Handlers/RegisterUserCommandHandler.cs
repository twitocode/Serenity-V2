using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common.Errors;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity.Handlers;

public record RegisterUserCommand(RegisterUserDto Dto) : IRequest<RegisterUserResponse>;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public RegisterUserCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (await userManager.FindByEmailAsync(command.Dto.Email) is not null)
        {
            return new RegisterUserResponse
            {
                Errors = new() { new("User Exists", "User already exists in the database") },
                Success = false
            };
        }

        User user = mapper.Map<User>(command.Dto);
        IdentityResult result = await userManager.CreateAsync(user, command.Dto.Password);

        if (result.Succeeded)
        {
            return new RegisterUserResponse
            {
                Errors = null,
                Success = true,
            };
        }

        List<ApplicationError> errors = new();

        foreach (var error in result.Errors.ToList())
        {
            errors.Add(new ApplicationError(error.Code, error.Description));
        }

        return new RegisterUserResponse
        {
            Errors = errors,
            Success = false,
        };
    }
}