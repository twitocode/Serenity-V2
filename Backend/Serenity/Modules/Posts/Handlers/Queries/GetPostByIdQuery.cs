using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetPostByIdQuery(string Id) : IRequest<Response<Post>>;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Response<Post>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetPostByIdQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<Post>> Handle(GetPostByIdQuery query, CancellationToken token)
    {
        var post = context.Posts.Where(x => x.Id == query.Id).FirstOrDefault();

        if (post is null)
        {
            return new()
            {
                Errors = new() { new("NoPostsFound", "There are no posts to mutate or query") },
                Data = null,
                Success = false
            };
        }

        return await Task.FromResult(new Response<Post>()
        {
            Errors = null,
            Success = true,
            Data = post
        });
    }
}
