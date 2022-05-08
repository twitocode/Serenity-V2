using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetPostsByTagQuery(int Page, string Tag) : IRequest<PaginatedResponse<List<Post>>>;

public class GetPostsByTagQueryHandler : IRequestHandler<GetPostsByTagQuery, PaginatedResponse<List<Post>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetPostsByTagQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Post>>> Handle(GetPostsByTagQuery query, CancellationToken token)
    {
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
        double pageCount = Math.Ceiling(context.Posts.Count() / postsPerPage);

        var posts = context.Posts
            .Where(x => x.Tags.Contains(query.Tag))
            .OrderByDescending(p => p.CreationTime)
            .Skip((query.Page - 1) * (int)postsPerPage)
            .Take((int)postsPerPage)
            .ToList();

        return await Task.FromResult(new PaginatedResponse<List<Post>>
        {
            CurrentPage = query.Page,
            Data = posts,
            Errors = null,
            Pages = (int)pageCount,
            Success = true,
        });
    }
}