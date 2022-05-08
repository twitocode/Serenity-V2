using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetMyPostsQuery(ClaimsPrincipal Claims, int Page) : IRequest<PaginatedResponse<List<Post>>>;

public class GetMyPostsQueryHandler : IRequestHandler<GetMyPostsQuery, PaginatedResponse<List<Post>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetMyPostsQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Post>>> Handle(GetMyPostsQuery query, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(query.Claims);

        if (user is null)
        {
            return new PaginatedResponse<List<Post>>
            {
                Success = false,
                Data = null,
                Errors = new()
                {
                    new("UserNotFound", "Could not find the user")
                }
            };
        }

        if (context.Posts.Count() == 0)
        {
            return new()
            {
                Errors = new() { new("NoPostsFound", "There are no posts to mutate or query") },
                Data = null,
                Success = false
            };
        }

        float postsPerPage = 10f;
        double pageCount = Math.Ceiling(context.Posts.Where(x => x.UserId == user.Id).Count() / postsPerPage);

        var posts = context.Posts
            .Where(x => x.UserId == user.Id)
            .OrderByDescending(p => p.CreationTime)
            .Skip((query.Page - 1) * (int)postsPerPage)
            .Take((int)postsPerPage)
            .ToList();

        return new PaginatedResponse<List<Post>>
        {
            CurrentPage = query.Page,
            Data = posts,
            Errors = null,
            Pages = (int)pageCount,
            Success = true,
        };
    }
}