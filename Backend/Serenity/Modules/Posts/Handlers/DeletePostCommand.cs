using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Posts.Handlers;

public record DeletePostCommand(string Id, ClaimsPrincipal Claims) : IRequest<Response<object>>;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Response<object>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public DeletePostCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<object>> Handle(DeletePostCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new()
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

        var post = context.Posts.Where(x => x.Id == command.Id && x.UserId == user.Id).First();

        if (post is null)
        {
            return new()
            {
                Success = false,
                Data = null,
                Errors = new() { new("PostNotFound", $"Could not find the post with Id of {command.Id}") }
            };
        }

        if (post.Comments is not null && post.Comments.Count() > 0)
        {
            foreach (Comment comment in post.Comments)
            {
                context.Comments.Remove(comment);
            }
        }

        user.Posts.Remove(post);
        context.Posts.Remove(post);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new()
            {
                Success = true,
                Errors = null,
                Data = null
            };
        }

        return new()
        {
            Data = null,
            Success = false,
            Errors = new() { new("DeletePostError", $"Could not delete the post with the Id of {command.Id}") }
        };
    }
}