using MediatR;
using Microsoft.AspNetCore.Identity;
using NodaTime;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetRecentPostsQuery(int Page) : IRequest<PaginatedResponse<List<Post>>>;

public class GetRecentPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, PaginatedResponse<List<Post>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetRecentPostsQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Post>>> Handle(GetRecentPostsQuery query, CancellationToken token)
    {
        float postsPerPage = 10f;
        double pageCount = Math.Ceiling(context.Posts.Count() / postsPerPage);

        var posts = context.Posts
            .Skip((query.Page - 1) * (int)postsPerPage)
            .OrderByDescending(p => p.CreationTime > SystemClock.Instance.GetCurrentInstant())
            .Take((int)postsPerPage)
            .ToList();

        return await Task.FromResult(new PaginatedResponse<List<Post>>
        {
            CurrentPage = query.Page,
            Data = posts,
            Errors = null,
            Pages = (int)pageCount
        });
    }
}