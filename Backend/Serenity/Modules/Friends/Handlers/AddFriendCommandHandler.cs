using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends.Handlers;

public record AddFriendCommand(ClaimsPrincipal Claims, string Id) : IRequest<Response>;

public class AddFriendCommandHandler : IRequestHandler<AddFriendCommand, Response>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public AddFriendCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response> Handle(AddFriendCommand command, CancellationToken token)
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
        var foundUser = await userManager.FindByIdAsync(command.Id);
        if (foundUser is null)
        {
            return new Response(false, new() { new("UserNotFound", $"Could not find the user with Id of {command.Id}") });
        }

        Friendship friendship = new Friendship
        {
            Users = new List<User> { user, foundUser }
        };

        var result = await context.SaveChangesAsync();

        if (context.Friendships.Count() > 0)
        {
            friendship = context.Friendships.Where(x => x.Users.Contains(foundUser)).First();
        }
        else
        {
            context.Friendships.Add(friendship);
        }

        user.Friendships.Add(friendship);
        foundUser.Friendships.Add(friendship);


        result = await context.SaveChangesAsync();

        if (result >= 0)
        {
            return new(true, null);
        }

        return new(false, new() { new("AddFriendError", "Could not add the friend") });
    }
}