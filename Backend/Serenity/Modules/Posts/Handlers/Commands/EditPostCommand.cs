using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Serenity.Common;
using Serenity.Database;
using Serenity.Database.Entities;
using Serenity.Modules.Posts.Dto;

namespace Serenity.Modules.Posts.Handlers;

public record EditPostCommand(EditPostDto Dto, ClaimsPrincipal Claims, string Id) : IRequest<Response<object>>;

public class EditPostCommandHandler : IRequestHandler<EditPostCommand, Response<object>>
{
    private readonly DataContext context;
    private readonly UserManager<User> userManager;
    private readonly IMapper mapper;

    public EditPostCommandHandler(IMapper mapper, DataContext context, UserManager<User> userManager)
    {
        this.mapper = mapper;
        this.userManager = userManager;
        this.context = context;
    }

    public async Task<Response<object>> Handle(EditPostCommand command, CancellationToken token)
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

        var post = context.Posts.Where(x => x.Id == command.Id && x.UserId == user.Id).FirstOrDefault();

        if (post is null)
        {
            return new()
            {
                Errors = new() { new("PostNotFound", $"Could not find the post with Id of {command.Id}") },
                Data = null,
                Success = false
            };
        }

        var updatedPost = mapper.Map<Post>(post);
        updatedPost.Content = command.Dto.Content;

        context.Entry(post).CurrentValues.SetValues(updatedPost);
        var result = context.SaveChanges();

        if (result >= 0)
        {
            return new(true, null, null);
        }

        return new()
        {
            Errors = new() { new("EditPostError", $"Could not edit the Post with the Id of {command.Id}") },
            Data = null,
            Success = false
        };
    }
}