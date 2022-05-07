using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database.Entities;

namespace Serenity.Modules.Identity.Handlers;

public record GetUserQuery(ClaimsPrincipal Claims) : IRequest<Response<User>> { }

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Response<User>>
{
    private readonly UserManager<User> userManager;

    public GetUserQueryHandler(UserManager<User> userManager) => this.userManager = userManager;

    public async Task<Response<User>> Handle(GetUserQuery command, CancellationToken cancellationToken)
    {
        User user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new()
            {
                Success = false,
                Errors = new() { new("UserNotFoundWithToken", "Could not find the user with the Token Provided") },
                Data = user
            };
        }

        return new()
        {
            Success = true,
            Errors = null,
            Data = user
        };
    }
}