using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common.Interfaces;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity.Handlers;

public record LoginUserCommand(LoginUserDto Dto) : IRequest<LoginUserResponse>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, LoginUserResponse>
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

    public async Task<LoginUserResponse> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        User user = await userManager.FindByEmailAsync(command.Dto.Email);

        if (user is null)
        {
            return new LoginUserResponse
            {
                Errors = new()
                {
                    new("UserNotFound", $"The user with the email of [{command.Dto.Email}] Does not exist")
                },
                Success = false,
                Token = null
            };
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, command.Dto.Password, false);

        if (!result.Succeeded)
        {
            return new LoginUserResponse
            {
                Errors = new()
                {
                    new("PasswordInvalid", "The password provided is not the same as the hashed password")
                },
                Success = false,
                Token = null
            };
        }

        return new LoginUserResponse
        {
            Errors = null,
            Success = true,
            Token = jwtService.GenerateToken(user)
        };
    }
}