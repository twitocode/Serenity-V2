using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database.Entities;

namespace Serenity.Modules.Identity.Handlers;

public record GetUserQuery(ClaimsPrincipal Claims) : IRequest<User> { }

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, User>
{
    private readonly UserManager<User> userManager;

    public GetUserQueryHandler(UserManager<User> userManager) => this.userManager = userManager;

    public async Task<User> Handle(GetUserQuery command, CancellationToken cancellationToken)
    {
        User user = await userManager.GetUserAsync(command.Claims);
        return user;
    }
}