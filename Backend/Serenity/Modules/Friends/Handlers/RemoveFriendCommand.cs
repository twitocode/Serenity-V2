using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends.Handlers;

public record RemoveFriendCommand(ClaimsPrincipal Claims, string Id) : IRequest<Response<object>>;

public class RemoveFriendCommandHandler : IRequestHandler<RemoveFriendCommand, Response<object>>
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

    public async Task<Response<object>> Handle(RemoveFriendCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new Response<object>
            {
                Success = false,
                Data = null,
                Errors = new()
                {
                    new("UserNotFound", "Could not find the user")
                }
            };
        }

        if (context.Friendships.Count() == 0)
        {
            return new()
            {
                Errors = new() { new("NoFriendshipsFound", "There are no friendships to mutate or query") },
                Data = null,
                Success = false
            };
        }

        var friendship = context.Friendships
            .Where(x => x.Users.Contains(user))
            .Include(x => x.Users)
            .First();

        if (friendship is null)
        {
            return new Response<object>(false, new() { new("FriendshipNotFound", $"Could not find the friendship") }, null);
        }

        var foundUser = friendship.Users.Where(x => x.Id == command.Id).First();

        user.Friendships.Remove(friendship);
        foundUser.Friendships.Remove(friendship);

        context.Friendships.Remove(friendship);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null, null);
        }

        return new(false, new() { new("RemoveFriendError", $"Could not remove the friend with the Id of {command.Id}") }, null);
    }
}