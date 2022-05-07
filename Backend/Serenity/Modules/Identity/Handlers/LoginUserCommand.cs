using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity.Handlers;

public record LoginUserCommand(LoginUserDto Dto) : IRequest<Response<string>>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Response<string>>
{
    private readonly IJwtService jwtService;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IMapper mapper;

    public LoginUserCommandHandler(UserManager<User> userManager, IJwtService jwtService, SignInManager<User> signInManager, IMapper mapper)
    {
        this.jwtService = jwtService;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<Response<string>> Handle(LoginUserCommand command, CancellationToken cancellationData)
    {
        User user = await userManager.FindByEmailAsync(command.Dto.Email);

        if (user is null)
        {
            return new()
            {
                Errors = new()
                {
                    new("UserNotFoundWithToken", $"Could not find the user with the email of {command.Dto.Email}")
                },
                Success = false,
                Data = null
            };
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, command.Dto.Password, false);

        if (!result.Succeeded)
        {
            return new()
            {
                Errors = new()
                {
                    new("InvalidPassword", "The password provided is not the same as the hashed password")
                },
                Success = false,
                Data = null
            };
        }

        return new()
        {
            Errors = null,
            Success = true,
            Data = jwtService.GenerateToken(user)
        };
    }
}