using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends.Handlers;

public record RemoveFriendCommand(ClaimsPrincipal Claims, string Id) : IRequest<Response>;

public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, Response>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public RemoveFriendCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response> Handle(RemoveFriendCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new Response
            {
                Errors = new()
                {
                    new("UserNotFound", "Could not find the user")
                }
            };
        }

        var friendship = context.Friendships
            .Where(x => x.Users.Contains(user))
            .Include(x => x.Users)
            .First();

        if (friendship is null)
        {
            return new Response(false, new() { new("FriendshipNotFound", $"Could not find the friendship") });
        }

        var foundUser = friendship.Users.Where(x => x.Id == command.Id).First();

        user.Friendships.Remove(friendship);
        foundUser.Friendships.Remove(friendship);

        context.Friendships.Remove(friendship);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null);
        }

        return new(false, new() { new("RemoveFriendError", "Could not add the friend") });
    }
}