using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
        var foundUser = context.Users.Where(x => x.Id == command.Id).First();

        if (foundUser is null)
        {
            return new Response(false, new() { new("UserNotFound", $"Could not find the user with Id of {command.Id}") });
        }

        user.Friends.Add(foundUser);
        foundUser.Friends.Add(user);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null);
        }

        return new(false, new() { new("AddFriendError", "Could not add the friend") });
    }
}