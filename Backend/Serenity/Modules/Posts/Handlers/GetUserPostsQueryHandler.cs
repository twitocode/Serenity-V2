using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetUserPostsQuery(ClaimsPrincipal Claims) : IRequest<List<Post>>;

public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, List<Post>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetUserPostsQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<List<Post>> Handle(GetUserPostsQuery query, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(query.Claims);
        var posts = context.Posts.Where(x => x.UserId == user.Id);

        return posts.ToList();
    }
}