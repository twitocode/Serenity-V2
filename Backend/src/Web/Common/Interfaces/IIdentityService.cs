using System.Security.Claims;
using Serenity.Database.Entities;
using Serenity.Modules.Auth.Dto;

namespace Serenity.Common.Interfaces;

public interface IAuthService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto dto);
    Task<LoginUserResponse> LoginAsync(LoginUserDto dto);
    Task<User> GetUserAsync(ClaimsPrincipal claims);
}