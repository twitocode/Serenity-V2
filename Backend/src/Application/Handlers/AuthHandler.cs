using System.Security.Claims;
using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Handlers;

public class AuthHandler
{
    private readonly IIdentityService identityService;

    public AuthHandler(IIdentityService identityService)
    {
        this.identityService = identityService;
    }

    public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto dto)
    {
        return await identityService.RegisterUserAsync(dto);
    }

    public async Task<LoginUserResponse> LoginAsync(LoginUserDto dto)
    {
        return await identityService.LoginAsync(dto);
    }

    public async Task<User> GetUserAsync(ClaimsPrincipal claims)
    {
        return await identityService.GetUserAsync(claims);
    }
}