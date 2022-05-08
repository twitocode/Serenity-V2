using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments.Handlers;

public record CreateCommentCommand(CreateCommentDto Dto, ClaimsPrincipal Claims, string PostId) : IRequest<Response<object>>;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Response<object>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public CreateCommentCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<object>> Handle(CreateCommentCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new Response<object>
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

        var post = context.Posts.Where(x => x.Id == command.PostId).FirstOrDefault();

        if (post is null)
        {
            return new()
            {
                Data = null,
                Errors = new() { new("PostNotFound", $"Could not find the post with Id of {command.PostId}") },
                Success = false
            };
        }

        var repliesTo = context.Comments
            .Where(x => x.Id == command.Dto.RepliesToId && x.PostId == command.PostId)
            .FirstOrDefault();

        if (repliesTo is null)
        {
            return new()
            {
                Data = null,
                Errors = new() { new("CommentNotFound", $"Could not find the comment with Id of {command.Dto.RepliesToId}") },
                Success = false
            };
        }

        var comment = new Comment
        {
            RepliesToId = command.Dto.RepliesToId,
            RepliesTo = repliesTo,
            Content = command.Dto.Content,
            User = user,
            UserId = user.Id.ToString(),
            PostId = post.Id,
            Post = post
        };

        post.Comments.Add(comment);
        context.Comments.Add(comment);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null, null);
        }

        return new(false, new() { new("CreateCommentError", "Could not create the Comment") }, null);
    }
}