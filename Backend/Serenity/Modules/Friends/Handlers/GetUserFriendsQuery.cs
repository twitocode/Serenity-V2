using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends.Handlers;

public record GetUserFriendsQuery(ClaimsPrincipal Claims) : IRequest<Response<List<Friendship>>>;

public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, Response<List<Friendship>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public GetUserFriendsQueryHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<List<Friendship>>> Handle(GetUserFriendsQuery command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (context.Friendships.Count() == 0)
        {
            return new()
            {
                Errors = new() { new("NoFriendshipsFound", "There are no friendships to mutate or query") },
                Data = null,
                Success = false
            };
        }

        var frienships = context.Friendships
            .Where(x => x.Users.Contains(user))
            .Include(p => p.Users)
            .ToList();

        return new()
        {
            Errors = null,
            Data = frienships,
            Success = true
        };
    }
}