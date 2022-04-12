using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

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
                Errors = new List<IdentityError> {
                    new IdentityError { Code = "UserNotFound", Description = $"The user with the email of [{dto.Email}] Does not exist" }
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
                Errors = new List<IdentityError> {
                    new IdentityError { Code = "PasswordInvalid", Description = "The password provided is not the same as the hashed password" },
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
                Errors = new List<IdentityError>() { new IdentityError { Code = "User Exists", Description = "User already exists in the database" } },
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

        return new RegisterUserResponse
        {
            Errors = result.Errors.ToList(),
            Success = false,
        };
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal claims)
    {
        User user = await userManager.GetUserAsync(claims);
        return user;
    }
}