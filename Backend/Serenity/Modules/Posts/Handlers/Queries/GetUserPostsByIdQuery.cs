using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetUserPostsByIdQuery(string Id, int Page) : IRequest<PaginatedResponse<List<Post>>>;

public class GetUserPostsByIdQueryHandler : IRequestHandler<GetUserPostsByIdQuery, PaginatedResponse<List<Post>>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetUserPostsByIdQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<PaginatedResponse<List<Post>>> Handle(GetUserPostsByIdQuery query, CancellationToken token)
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
        double pageCount = Math.Ceiling(context.Posts.Where(x => x.UserId == query.Id).Count() / postsPerPage);

        var posts = context.Posts
            .Where(x => x.UserId == query.Id)
            .Skip((query.Page - 1) * (int)postsPerPage)
            .OrderByDescending(p => p.CreationTime)
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