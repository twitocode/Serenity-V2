using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;

namespace Serenity.Modules.Comments.Handlers;

public record DeleteCommentCommand(string CommentId, ClaimsPrincipal Claims, string PostId) : IRequest<Response<object>>;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Response<object>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public DeleteCommentCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<object>> Handle(DeleteCommentCommand command, CancellationToken token)
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

        if (context.Comments.Count() == 0)
        {
            return new()
            {
                Errors = new() { new("NoCommentsFound", "There are no comments to mutate or query") },
                Data = null,
                Success = false
            };
        }

        var comment = context.Comments
            .Where(x => x.Id == command.CommentId && x.UserId == user.Id && x.PostId == command.PostId)
            .Include(x => x.Post)
            .Include(x => x.Replies)
            .First();

        if (comment is null)
        {
            return new()
            {
                Success = false,
                Errors = new() { new("CommentNotFound", $"Could not find the comment with the Id of {command.CommentId}") }
            };
        }

        if (comment.Replies is not null)
        {
            foreach (var reply in comment.Replies)
                context.Comments.Remove(reply);
        }

        comment.Post.Comments.Remove(comment);
        context.Comments.Remove(comment);

        var result = context.SaveChanges();

        if (result >= 0)
            return new()
            {
                Success = true,
                Errors = null
            };

        return new()
        {
            Success = false,
            Errors = new() { new("DeleteCommentError", $"Could not delete the comment with the Id of {command.CommentId}") }
        };
    }
}