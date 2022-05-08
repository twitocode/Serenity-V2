using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments.Handlers;

public record EditCommentCommand(EditCommentDto Dto, ClaimsPrincipal Claims, string PostId) : IRequest<Response<object>>;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand, Response<object>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public EditCommentCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<object>> Handle(EditCommentCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new Response<object>
            {
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

        else if (context.Comments.Count() == 0)
        {
            return new()
            {
                Errors = new() { new("NoCommentsFound", "There are no comments to mutate or query") },
                Data = null,
                Success = false
            };
        }

        var comment = context.Comments
            .Where(x => x.Id == command.Dto.CommentId && x.UserId == user.Id && x.PostId == command.PostId)
            .First();

        if (comment is null)
        {
            return new()
            {
                Success = false,
                Data = null,
                Errors = new()
                {
                    new("CommentNotFound", $"Could not find the comment with the Id of {command.Dto.CommentId}")
                }
            };
        }

        var updatedComment = mapper.Map<Comment>(comment);
        updatedComment.Content = command.Dto.Content;

        context.Entry(comment).CurrentValues.SetValues(updatedComment);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null, null);
        }

        return new()
        {
            Success = false,
            Data = null,
            Errors = new()
            {
                new("EditCommentError", $"Could not edit the Comment with the Id of {command.Dto.CommentId}")
            }
        };
    }
}