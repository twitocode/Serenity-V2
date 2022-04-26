using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record GetPostByIdQuery(string Id) : IRequest<Post>;

public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, Post>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;

    public GetPostByIdQueryHandler(DataContext context, UserManager<User> userManager)
    {
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Post> Handle(GetPostByIdQuery query, CancellationToken token)
    {
        var post = context.Posts.Where(x => x.UserId == query.Id).FirstOrDefault();
        return post;
    }
}