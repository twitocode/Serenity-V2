using System.Security.Claims;
using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces;

public interface IIdentityService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto dto);
    Task<LoginUserResponse> LoginAsync(LoginUserDto dto);
    Task<User> GetUserAsync(ClaimsPrincipal claims);
}