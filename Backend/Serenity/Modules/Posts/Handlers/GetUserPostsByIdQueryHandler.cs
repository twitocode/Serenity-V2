using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetUserPostsByIdQuery(string Id) : IRequest<List<Post>>;

public class GetUserPostsByIdQueryHandler : IRequestHandler<GetUserPostsByIdQuery, List<Post>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetUserPostsByIdQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<List<Post>> Handle(GetUserPostsByIdQuery query, CancellationToken token)
    {
        var posts = context.Posts.Where(x => x.UserId == query.Id);
        return await Task.FromResult(posts.ToList());
    }
}