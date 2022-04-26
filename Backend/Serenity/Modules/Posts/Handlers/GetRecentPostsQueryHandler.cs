using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetRecentPostsQuery : IRequest<List<Post>>;

public class GetRecentPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, List<Post>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetRecentPostsQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<List<Post>> Handle(GetRecentPostsQuery query, CancellationToken token)
    {
        var posts = context.Posts.Where(x => true).OrderByDescending(x => true);
        return posts.ToList();
    }
}