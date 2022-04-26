using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Friends.Handlers;

public record GetUserFriendsQuery(ClaimsPrincipal Claims) : IRequest<List<User>>;

public class GetUserFriendsQueryHandler : IRequestHandler<GetUserFriendsQuery, List<User>>
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

    public async Task<List<User>> Handle(GetUserFriendsQuery command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);
        return user.Friends;
    }
}