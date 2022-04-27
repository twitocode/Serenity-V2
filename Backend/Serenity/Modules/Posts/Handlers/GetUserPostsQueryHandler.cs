using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetUserPostsQuery(ClaimsPrincipal Claims, int Page) : IRequest<PaginatedResponse<List<Post>>>;

public class GetUserPostsQueryHandler : IRequestHandler<GetUserPostsQuery, PaginatedResponse<List<Post>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetUserPostsQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Post>>> Handle(GetUserPostsQuery query, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(query.Claims);

        float postsPerPage = 10f;
        double pageCount = Math.Ceiling(context.Posts.Where(x => x.UserId == user.Id).Count() / postsPerPage);

        var posts = context.Posts
            .Where(x => x.UserId == user.Id)
            .Skip((query.Page - 1) * (int)postsPerPage)
            .Take((int)postsPerPage)
            .ToList();

        return new PaginatedResponse<List<Post>>
        {
            CurrentPage = query.Page,
            Data = posts,
            Errors = null,
            Pages = (int)pageCount
        };
    }
}