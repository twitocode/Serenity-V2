using System.Security.Claims;
using Serenity.Database.Entities;
using Serenity.Modules.Identity.Dto;

namespace Serenity.Common.Interfaces;

public interface IIdentityService
{
    Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto dto);
    Task<LoginUserResponse> LoginAsync(LoginUserDto dto);
    Task<User> GetUserAsync(ClaimsPrincipal claims);
}