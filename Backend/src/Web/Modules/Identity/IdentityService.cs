using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Serenity.Common.Errors;
using Serenity.Common.Interfaces;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Modules.Identity;

public class IdentityService : IIdentityService
{
    private readonly DataContext context;
    private readonly IJwtService jwtService;
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;
    private readonly IMapper mapper;

    public IdentityService(DataContext context, UserManager<User> userManager, IJwtService jwtService, SignInManager<User> signInManager, IMapper mapper)
    {
        this.jwtService = jwtService;
        this.context = context;
        this.signInManager = signInManager;
        this.userManager = userManager;
        this.mapper = mapper;
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserDto dto)
    {
        User user = await userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            return new LoginUserResponse
            {
                Errors = new List<ApplicationError>
                {
                    new ApplicationError("UserNotFound", $"The user with the email of [{dto.Email}] Does not exist")
                },
                Success = false,
                Token = null
            };
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (!result.Succeeded)
        {
            return new LoginUserResponse
            {
                Errors = new List<ApplicationError>
                {
                    new ApplicationError("PasswordInvalid", "The password provided is not the same as the hashed password")
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

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto dto)
    {
        if (await userManager.FindByEmailAsync(dto.Email) is not null)
        {
            return new RegisterUserResponse
            {
                Errors = new List<ApplicationError>()
                {
                    new ApplicationError("User Exists", "User already exists in the database")
                },
                Success = false
            };
        }

        User user = mapper.Map<User>(dto);
        IdentityResult result = await userManager.CreateAsync(user, dto.Password);

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

    public async Task<User> GetUserAsync(ClaimsPrincipal claims)
    {
        User user = await userManager.GetUserAsync(claims);
        return user;
    }
}