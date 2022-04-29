using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Comments.Dto;

namespace Serenity.Modules.Comments.Handlers;

public record CreateCommentCommand(CreateCommentDto Dto, ClaimsPrincipal Claims, string PostId) : IRequest<CreateCommentResponse>;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
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

    public async Task<CreateCommentResponse> Handle(CreateCommentCommand command, CancellationToken token)
    {
        var user = await userManager.GetUserAsync(command.Claims);

        if (user is null)
        {
            return new CreateCommentResponse
            {
                Errors = new()
                {
                    new("UserNotFound", "Could not find the user")
                }
            };
        }

        var post = context.Posts.Where(x => x.Id == command.PostId).First();

        if (post is null)
        {
            return new CreateCommentResponse(false, new() { new("CommentNotFound", $"the post with the Id of {command.PostId} does not exist") });
        }

        var comment = new Comment
        {
            RepliesToId = command.Dto.RepliesToId,
            Content = command.Dto.Content,
            User = user,
            UserId = user.Id.ToString(),
            PostId = post.Id,
            Post = post
        };

        context.Comments.Add(comment);
        post.Comments.Add(comment);

        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new CreateCommentResponse(true, null);
        }

        return new CreateCommentResponse(false, new() { new("CreatePostError", "Could not create the Post") });
    }
}